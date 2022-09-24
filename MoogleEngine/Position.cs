
namespace loader;
//los objetos de tipo position guardan dos valores la linea del documento(line) y su lugar en esa linea(place)
public class Position{

public int line;
public int place;

public Position(int row , int dir){

 this.line = row;
 this.place = dir;

}


}
