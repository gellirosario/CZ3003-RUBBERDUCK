using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class User 
{
    public string userID;
    public string name;
    public string email;
    public string userType;
    public Player player;

    public User() {
        
    }
    
    public User(string id, string name, string email, string userType)
    {
        this.userID = id;
        this.name = name;
        this.email = email;
        this.userType = userType;
        this.player = null;
    }
    
    public User(string id, string name, string email, string userType, Player player)
    {
        this.userID = id;
        this.name = name;
        this.email = email;
        this.userType = userType;
        this.player = player;
    }
    
    // Updating IDictionary to Class
    public User(IDictionary <string, object> dict)
    {
        //pls refer to question model 
    }
}
