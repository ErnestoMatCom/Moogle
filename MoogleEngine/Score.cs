using loader;

namespace MoogleEngine;
public class Score{

//calcula el score de los documentos

 public static float TFIDF(string word , int doc){//calcula el peso segun TFIDF

float idf = IDF(word);

  if(idf != 0.0f && Loader.matrix[doc].ContainsKey(word)){

  return Loader.matrix[doc][word].Count * idf;//multiplica el IDF por la frecuencia bruta del termino( TF )

  }else{

   return 0.0f;

  }

 }

private static float IDF(string word){//calcula el IDF

 
 float documents = 0.0f ;


 for(int c = 0;c < Loader.documents.Length;c++){

   if(Loader.matrix[c].ContainsKey(word) ){

       documents++;

     }

   }

  if(documents != 0){ 
       return (float)Math.Log10(Loader.documents.Length / documents);
  }else{

    return 0.0f;

  }

}

//calcula la similitud cosenica de las palabras de la busqueda con un determinado documento
public static float score( string[] search , int doc ){
  

  float similitud = M(search, doc ) / ( M(search) * M( Loader.weight[doc].Values.ToArray() ) );//similitud cosenica


 return similitud;

}

public static float M(float[] search){//metodo sobrecargado que calcula las sumatorias de terminos para la similitud cosenica

 float result = 0.0f;

 foreach(float elem in search){

   result += elem * elem; 

 }

 return (float)Math.Sqrt(result);

}


public static float M(string[] search, int doc){

  

 IDictionary<string ,int > n = new Dictionary<string , int>();
 float similitud = 0.0f;

 foreach(string elem in search){

   n.Add(elem,0);
   foreach(string element in search){

    if(elem == element){ 

   n[elem] += 1;

    }

   }
 }

 foreach(string elem in search){

  try{

   similitud += (float)( n[elem] ) * Loader.weight[doc][elem];

  }catch{
   
   similitud += 0;

  }


 }

 return similitud;

  

}




public static float M(string[] search){

 IDictionary<string ,int > n = new Dictionary<string , int>();
 float similitud = 0.0f;

 foreach(string elem in search){

   n.Add(elem,0);
   foreach(string element in search){

    if(elem == element){ 

   n[elem] += 1;

    }

   }
 }

 foreach(string elem in search){

   similitud += n[elem] * n[elem];

 }

 return (float)Math.Sqrt(similitud);

}



public static string snippet(string word ,int doc){//calcula el snippet utilizando el primer elemento de la lista de posiciones de la clase Loader

StreamReader reader = new StreamReader(Loader.documents[doc]);
string snippet = "";

for(int c = 0;c <= Loader.matrix[doc][word][0].line;c++){

 snippet = reader.ReadLine();

}

reader.Close();
return snippet;
}

}