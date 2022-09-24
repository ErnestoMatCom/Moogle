using loader;
namespace MoogleEngine;

//clase para encontrar la palabra mas cercana a la buscada

public class Distance{

 public static int lev(string a , string b){

   //este metodo calcula la distancia de levenshtein entre dos palabras

    if(b.Length == 0)return a.Length;
    if(a.Length == 0)return b.Length;

    if(a[0] == b[0]){

    return lev(a.Remove(0,1) ,b.Remove(0,1));

    }else{

      return 1 + Math.Min( Math.Min(  lev(a.Remove(0,1),b) , lev(a , b.Remove(0,1)  )),lev(a.Remove(0,1),b.Remove(0,1) ) );

    }

 }


public static string minDist(string word){
//este metodo encuentra la palabra con la menor distancia en todos los documentos

ICollection<string> keys = Loader.matrix[0].Keys;
int dist = lev( keys.ElementAt(0) , word );

string closest = keys.ElementAt(0);

for( int c = 0 ;c < Loader.matrix.Length;c++){
  keys = Loader.matrix[c].Keys;


  foreach( string w in keys ){

   if( lev(w , word ) < dist){

       dist = lev( w , word );
       closest = w;

    
   }

  }
}

return closest;

}


}

