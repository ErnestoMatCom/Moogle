
/*

Esta clase realizara el calculo de el score segun operadores 
el metodo principal es operatorValue que al principio 
revisa si existe un operador delante de la palabra 
para esto recibe varios parametros 

query: la frase a buscar

word: la palabra a comprobar si tiene operadores

nextWord: para el operador de cercania, en los demas operadores comprueba si el operador esta a la izquierda, en este comprueba si esta a la derecha

rep: en caso de que word este repetido para comprobar el operador de la repeticion correcta y que IndexOf no compruebe el operador de una repeticion anterior

document: el documento a revisar esto es para todos los operadores menos el de importancia  *   

*/

using loader;

namespace MoogleEngine;

public class Operator{

public static float operatorValue(string query , string word , string nextWord, int rep , IDictionary<string, List<Position> > document ){

  float value = 0.0f;//score final segun operadores
  int position  = 0;//posicion de la palabra en el query
  string subQuery ;//esta variable es para el caso de que se repita la palabra
  char op ;//esto guarda el operador
  float importance = 0.0f;//para el operador de importancia *

//esto para en caso de que la palabra se repita este bucle solo tomara lo que existe desde esta repeticion de word hasta el final
//y comprueba los operadores anteriores a este subString

  if(rep > 1 ){

   for( int c = 0;c < rep;c++ ){

     subQuery = query.Substring(position + 1);

     position = subQuery.IndexOf(word);   
  

   }//fin de for

  }else{

   position = query.IndexOf(word);  

  }//fin de if-else


//si la posicion de la palabra es 0 ( o sea esta al principio de query entonces no hay operador y comprobar si existe genera un error  )
//en caso contrario guarda el caracter anterior a word
  if( position != 0 ){ 
  op = query[position - 1];

  }else{
 
   op = '&';
   value = 1.0f;

  }



  switch(op){

   case '!'://retorna 0 si la palabra esta en el documento

   value = isNot(word ,document)? 1.0f:0.0f;

   break;

   case '^'://contrario de el operador anterior

   value = ( !isNot(word ,document) )? 1.0f:0.0f;

   break;

   case '*'://operador de importancia, retorna la cantidad de "*" mas 1
   
   
    for( int c = 0; query[position - 1 - c] == '*'; c++ ){

      importance++;

    }

    value = importance + 1.0f;

   break;


   default://en caso de que ninguno de estos operadores exista retorna 1

    value = 1;

   break;

  }



try{//como el operador de cercania comprueba si esta al final de la palabra y los demas al principio entonces esta fuera del switch

if( query[ position + word.Length ] == '~' ){

value = 100 / close(document[word].ToArray() , document[nextWord].ToArray() );//si esto genera error es que la palabra no esta en el documento entonces retorna 1

}

}catch{

value = 1.0f;

}


  
return value;
  
}



private static float close( Position[] left , Position[] right ){//comprobar cercania


  int r = 0;
  int l = 0;

  float minDist = Math.Abs( left[l].line - right[r].line ) ;

  //este metodo comprueba los numeros de las dos listas de posiciones y toma la menor diferencia entre las posiciones de las dos 
  
  for(int c = 0 ; c < (left.Length + right.Length) ; c++ ){

    if(right[r].line >= left[l].line){

      r++;
      if( Math.Abs( left[l].line - right[r].line) < minDist ){

       minDist = Math.Abs( left[l].line - right[r].line ) ;

      }

    }else{

       l++;

       if( Math.Abs( left[l].line - right[r].line) < minDist ){

       minDist = Math.Abs( left[l].line - right[r].line ) ;

      }   

    }//fin de if-else

  }//fin de for

  return minDist;



}


private static bool isNot(string w , IDictionary<string,List<Position> > doc){//comprueba si la palabra esta en el documento esta en un metodo aparte pra mayor comodidad 

   bool val;

  if( doc.ContainsKey(w) ){

      val = false;


  }else{

    val = true;

  }

  return val;

}


}



