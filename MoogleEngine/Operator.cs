
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

public static float operatorValue(string query , string word , string nextWord, int position , IDictionary<string, List<Position> > document ){

  float value = 0.0f;//score final segun operadores
  char op ;//esto guarda el operador
  float importance = 0.0f;//para el operador de importancia *
 

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
   
   if(document.ContainsKey(word)){

    for( int c = 0; position -1 - c >=0 && query[position - 1 - c] == '*' ; c++ ){//*********

      importance++;

    }

    value = importance + 1.0f;

   }else{

     value = 1;

   }

   break;


   default://en caso de que ninguno de estos operadores exista retorna 1

    value = 1;

   break;

  }

  

//como el operador de cercania comprueba si esta al final de la palabra y los demas al principio entonces esta fuera del switch

if((query.Length > position + word.Length) && (nextWord != word) && (query[ position + word.Length  ] == '~' )){

try{

value =  close(document[word].ToArray() , document[nextWord].ToArray() );//si esto genera error es que la palabra no esta en el documento entonces retorna 0

}catch{

value = 0.0f;

}

}


return value;
  
}



private static float close( Position[] left , Position[] right ){//comprobar cercania


  int r = 0;
  int l = 0;

  float minDist = Math.Abs( left[l].line - right[r].line ) ;

  //este metodo comprueba los numeros de las dos listas de posiciones y toma la menor diferencia entre las posiciones de las dos 
  
  for(int c = 0 ; c < (left.Length + right.Length) ; c++ ){
    
    if( ( r < right.Length ) && ( l < left.Length ) ){

    if( right[r].line >= left[l].line && r < right.Length ){


      

      if( Math.Abs( left[l].line - right[r].line) < minDist ){

       minDist = Math.Abs( left[l].line - right[r].line ) ;

      }

     r++;
    }
    
    if(right[r].line < left[l].line && l < left.Length ){
       

       if( Math.Abs( left[l].line - right[r].line) < minDist ){

       minDist = Math.Abs( left[l].line - right[r].line ) ;

      }   

     l++;

    }

  }

  

  }//fin de for

  return 1/minDist;



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



