
//utilizado para comprobar la similitud de dos objetos de tipo SearchItem segun su Score
using System.Collections;

namespace MoogleEngine;

public class ResultComparer : IComparer{

  public int Compare(object x , object y){

    return ( new CaseInsensitiveComparer()).Compare(((SearchItem)x).Score,((SearchItem)y).Score);

  }

   

}

