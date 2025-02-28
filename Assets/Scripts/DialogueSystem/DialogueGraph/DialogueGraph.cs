using UnityEngine;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    [CreateAssetMenu(menuName = "Dialogue Editor/New Dialogue", order = 0)]
    public class DialogueGraph : NodeGraph
    {
        public StartNode StartNode => nodes.Find(x => x is StartNode) as StartNode;
    }
}
