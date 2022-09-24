using loader;
/*
metodo que hace la busqueda
y llama a todas las clases que calculan el score,
y generan el snippet y la sugerencia
y crea el array de resultados
*/

namespace MoogleEngine;

public static class Moogle
{
    public static SearchResult Query(string query) {

     char[] separators = new char[]{' ','\\','*','/',',','.',';','-','`',':','"','>','<','_','[',']','{','}','(',')','^','%','$','#','@','!','~'};//ignorar estos caracteres
     string[] search = query.Split(separators,StringSplitOptions.RemoveEmptyEntries);//separar las palabras del query
     int pos;//posicion de la palabra a buscar
     string subQuery = query;//para construir la sugerencia se elimina desde esta repeticion de la palabra hasta el principio de la query por si acaso se repite la palabra
     float[] finalScore = new float[Loader.documents.Length];//score de cada documento
     string suggestion = "";//para crear la sugerencia
     bool found = false;//para comprobar si la palabra se encontro, si no, entonces buscar la mas parecida
     SearchItem[] items = new SearchItem[Loader.documents.Length];//array de resultados
     IDictionary<string,int> rep = new Dictionary<string,int>();//cantidad de veces que se repite la palabra( para los operadores )
     int position = 0;
     float score;//score inicial de cada documento
     string[] snippet = new string[Loader.documents.Length];


      for(int r = 0;r < search.Length;r++){//recorre cada palabra en la busqueda
          
          pos = subQuery.IndexOf(search[r]);


          if(rep.ContainsKey(search[r] )){//comprueba si la palabra se repite

            rep[ search[r] ] += 1;

          }else{

          rep.Add(search[r] , 1);

          }


          if(rep[search[r]] > 1 ){  //si se repite toma el substring desde esa repeticion hasta el final

   for( int c = 0; c < rep[search[r]];c++ ){

     subQuery = query.Substring(position + 1);

     pos = subQuery.IndexOf(search[r]);   
  

   }//fin de for

  }else{

   pos = query.IndexOf(search[r]);  

  }



        for(int c = 0;c < Loader.documents.Length;c++){//recorre todos los documentos

         score = Score.score( search , c ) ;//halla el score de ese documentos

          try{//calcula el score segun operadores si es la ultima palabra genera error y entoces envia la misma a el metodo score

          finalScore[c] = (finalScore[c] + score) * Operator.operatorValue(query , search[r] , search[r+1] , rep[search[r]] , Loader.matrix[c] );

          }catch{

           finalScore[c] = (finalScore[c] + score) * Operator.operatorValue(query , search[r] , search[r] , rep[search[r]] , Loader.matrix[c] );
          
          }
          

          try{//crea el snippet de la ultima palabra de la busqueda en ese documento si no esta da error y  deja el snippet como esta hasta ese momento

            snippet[c] = Score.snippet( search[r] , c );
            items[c] =  new SearchItem(Loader.documents[c].Substring(1 + Loader.documents[c].LastIndexOf("/")), snippet[c] , finalScore[c]);
            found = true;

          }catch{

              
             items[c] =  new SearchItem(Loader.documents[c].Substring(1 + Loader.documents[c].LastIndexOf("/")), snippet[c] , finalScore[c]);
              found = false;
             
          }
          
        
       }

        if( found ){//crea la sugerencia, si la palabra no esta busca la mas cercana y la añade a la sugerencia junto con todo lo que hay de esa palabra hasta la siguiente o hasta el final

           suggestion = suggestion +  query.Substring(position, pos - position ) + search[r] ;

           position = query.IndexOf(search[r]) + search[r].Length;

         }else{
           
           suggestion = suggestion + Distance.minDist(search[r]) ;

           position = subQuery.IndexOf(search[r]) + search[r].Length  ;  

           if(r + 1 < search.Length ){

           suggestion = suggestion + subQuery.Substring( position, subQuery.IndexOf( search[r + 1] ) - position );

 
           }else{              

             suggestion = suggestion + subQuery.Substring(position);

           }
         }


       found = false;
       subQuery = query;

       pos = 0;
       
      }

      
      List<SearchItem> newItems = new List<SearchItem>();

      
      for(int c = 0 ; c < items.Length ; c++){//crea una lista con los documentos con score no nulo
      
      if(items[c].Score != 0.0f ){

        newItems.Add(items[c] );

      }

      }

      items = newItems.ToArray();//convierte la lista en array


      Array.Sort( items, new ResultComparer() );//ordena los documentos segun score de menor a mayor
      Array.Reverse(items);//canbia el orden a uno de mayor a menor



       return new SearchResult(items, suggestion);


    }   



}
