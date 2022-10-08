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
     int[] indice = new int[search.Length];//posicion de cada palabra
     string subQuery = query;//para construir la sugerencia se elimina desde esta repeticion de la palabra hasta el principio de la query por si acaso se repite la palabra
     float[] finalScore = new float[Loader.documents.Length];//score de cada documento
     float[] fScore = new float[finalScore.Length];
     for(int c = 0;c < fScore.Length;c++)fScore[c] = 1;
     string suggestion = "";//para crear la sugerencia
     bool found = false;//para comprobar si la palabra se encontro, si no, entonces buscar la mas parecida
     SearchItem[] items = new SearchItem[Loader.documents.Length];//array de resultados
     IDictionary<string,int> rep = new Dictionary<string,int>();//cantidad de veces que se repite la palabra( para los operadores )
     float score;//score inicial de cada documento
     string[] snippet = new string[Loader.documents.Length];
     float op;


      for(int r = 0;r < search.Length;r++){//recorre cada palabra en la busqueda

          if(r == 0){
            
            indice[r] = query.IndexOf(search[r]);

          }else{

          indice[r] = subQuery.IndexOf(search[r]) + indice[r - 1] + search[r-1].Length ;

          }

          if(r < search.Length - 1){

          subQuery = query.Substring(indice[r] + search[r].Length );

          }

         

        for(int c = 0;c < Loader.documents.Length;c++){//recorre todos los documentos

         score = Score.score( search , c ) ;//halla el score de ese documentos

         if(search.Length > r + 1  ){
 
         op = Operator.operatorValue(query , search[r] , search[r+1] , indice[r] , Loader.matrix[c] );

         

         }else{

          op = Operator.operatorValue(query , search[r] , search[r] , indice[r] , Loader.matrix[c] );


         }

        //calcula el score segun operadores s

          if( op == 0.0f ){

          fScore[c] = 0;

          }else{


          finalScore[c] = finalScore[c] + score * op;

          
          }
         
          

          try{//crea el snippet de la ultima palabra de la busqueda en ese documento si no esta da error y  deja el snippet como esta hasta ese momento

            snippet[c] = Score.snippet( search[r] , c );
            items[c] =  new SearchItem(Loader.documents[c].Substring(1 + Loader.documents[c].LastIndexOf("/")), snippet[c] , finalScore[c] * fScore[c]);
            found = true;

          }catch{

              
             items[c] =  new SearchItem(Loader.documents[c].Substring(1 + Loader.documents[c].LastIndexOf("/")), snippet[c] , finalScore[c] * fScore[c]);
              found = false;
             
          }
          
        
       }

        if( found ){//crea la sugerencia, si la palabra no esta busca la mas cercana y la añade a la sugerencia junto con todo lo que hay de esa palabra hasta la siguiente o hasta el final
 
           if(r == 0){

              suggestion = query.Substring(0, search[r].Length ) ;


           }else{

             if(r < search.Length - 1){

            suggestion = suggestion +  query.Substring(indice[r - 1] + search[r-1].Length,indice[r - 1] + search[r-1].Length + search[r].Length ) ;

             }else{

              try{

              suggestion = suggestion +  query.Substring( indice[r - 1] + search[r-1].Length );

              }catch{

                suggestion = suggestion;

              }

             }


           }

         }else{

           if(r != 0){

           suggestion = suggestion + query.Substring( indice[r - 1] + search[r - 1].Length , indice[r] - indice[r - 1] - search[r - 1].Length  ) +  Distance.minDist(search[r]) ;
            

             }else{

            suggestion = query.Substring(0,indice[r]) +  Distance.minDist(search[r]) ;

             }
           
 

         }


       
       
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
