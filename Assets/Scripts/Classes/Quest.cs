using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Classes {
    public class Quest // Quest can be a Trigger as well
    {
        public int QuestID {get; private set;} // use it to search in a .json file
        public Quest[] QuestRequirements {get; private set;}

        public Quest(int questID, Quest[] questRequirements) {
            QuestID = questID;
            questRequirements.CopyTo(QuestRequirements, 0);
        }
    }
}

