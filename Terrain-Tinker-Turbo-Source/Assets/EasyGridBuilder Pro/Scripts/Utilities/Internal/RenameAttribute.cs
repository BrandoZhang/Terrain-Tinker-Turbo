using UnityEngine;
 
namespace SoulGames.Utilities
{
    public class RenameAttribute : PropertyAttribute
    {
        public string NewName { get ; private set; }    
        public RenameAttribute( string name )
        {
            NewName = name ;
        }
    }
}
