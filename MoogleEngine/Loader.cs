/*
Todo este namespace se encarga de cargar los documentos 
y colocar los datos necesarios en las estructuras 
*/

using MoogleEngine;


namespace loader;

public class Loader{

  public static IDictionary< string, List<Position> >[] matrix;//array de diccionarios que representan documentos como pares de palabras con una lista de posiciones
  public static string[] documents;//rutas de documentos

  public static IDictionary<string , float >[] weight;//peso de documentos

//metodo principal que crea 
//la estructura inicial donde se guardan los datos
 public static void mainLoader(){

   
   documents = Directory.GetFiles(@"../Content/");//inicializa array de rutas y nombres de documentos
   if(documents.Length != 0){//si no hay documentos
   
   weight = new Dictionary< string , float >[ documents.Length ]; //array de pesos de cada palabra en cada documento
   
   matrix = new Dictionary< string,List<Position> >[ documents.Length ];//inicializa el array de documentos

  string[] words;//guarda las palabras de la linea a procesar
  char[] separators = new char[]{' ',',','.',';','\'','-','`',':','"','>','<','_','[',']','{','}','(',')','*','&','^','%','$','#','@','!','~'};


for(int d = 0; d < documents.Length;d++){//recorre cada documento en el array matrix

  matrix[d] = new Dictionary< string,List<Position> >();//inicializa cada elemento de matrix
  StreamReader reader = new StreamReader(documents[d]);

  string line = reader.ReadLine();//lee cada linea
  
  int row = 0;//guarda en que linea se encuentra
  
  while(line != null){

  words =  line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
 
 
  for(int p = 0;p < words.Length;p++){//si existe la palabra colocar nueva posicion en la lista si no crear nuevo par palabra - lista de posiciones
  
   if(matrix[d].ContainsKey(words[p].ToLower()) ){//si existe

    matrix[d][words[p].ToLower()].Add(new Position(row,p));

   }else{//si no existe

    matrix[d].Add( words[p].ToLower(), new List<Position>() );
    matrix[d][words[p].ToLower()].Add(new Position(row,p));

   }
  
  }

  line = reader.ReadLine();
  row++;

  }

reader.Close();//cierra el stream
}

//carga el peso de los documentos
for(int d = 0 ; d < Loader.matrix.Length ; d++){

   weight[d] = new Dictionary<string,float>(); 

   string[] wordDoc = Loader.matrix[d].Keys.ToArray(); 


   for(int c = 0 ; c < wordDoc.Length ; c++ ){

   weight[d].Add( wordDoc[c] , Score.TFIDF(wordDoc[c],d));

   }
   }
    
   }else{
   
throw new Exception("NO HAY DOCUMENTOS");

   }
 }

}

