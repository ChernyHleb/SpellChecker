using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker
{
    public class BKTreeNode
    {
        public string Word;
        /// Key is Edit Distance
        /// Value is node that contains word
        public Dictionary<int, BKTreeNode> Children;
        public int ChildrenAmount
        {
            get
            {
                if (Children == null)
                    return 0;
                else
                    return Children.Count();
            }
        }

        public BKTreeNode(string Word)
        {
            this.Word = Word;
        }

        public void AddChildNode(int EditDistance ,string word)
        {
            if (this.Children == null)
                this.Children = new Dictionary<int, BKTreeNode>();
            this.Children.Add(EditDistance, new BKTreeNode(word));
        }
        public bool ContainsDistance(int EditDistance)
        {
            if (this.Children == null)
                return false;
            if (this.Children.ContainsKey(EditDistance))
                return true;
            else
                return false;
        }
    }

    public class BK_Tree
    {
        private BKTreeNode RootNode;
        private BKTreeNode NodePointer;

        public BK_Tree(string RootWord)
        {
            RootNode = new BKTreeNode(RootWord);
            NodePointer = RootNode;
        }

        public void AddNode(string Word)
        {
            NodePointer = RootNode;
            int EditDistance = SpellChecker.FindMinEditDistance(NodePointer.Word, Word);

            while(NodePointer.ContainsDistance(EditDistance))
            {
                NodePointer = NodePointer.Children[EditDistance];
                EditDistance = SpellChecker.FindMinEditDistance(NodePointer.Word, Word);
            }
            NodePointer.AddChildNode(EditDistance, Word);
        }

        public Dictionary<string, int> FindMatches(string Word, int accuracy)
        {
            Dictionary<string, int> Matches = new Dictionary<string, int>();
            Search(RootNode, Matches, Word, accuracy);
            return Matches;
        }

        private void Search(BKTreeNode NodePointer, Dictionary<string, int> Matches, 
                           string Word, int accuracy)
        {
            int EditDistance = SpellChecker.FindMinEditDistance(NodePointer.Word, Word);
            int leftBorder = EditDistance - accuracy;
            int rightBorder = EditDistance + accuracy;

            if (EditDistance <= accuracy)
                Matches.Add(NodePointer.Word, EditDistance);

            if(NodePointer.Children != null)
                foreach(var Child in  NodePointer.Children)
                {
                    if(Child.Key >= leftBorder && Child.Key <= rightBorder)
                        Search(Child.Value, Matches, Word, accuracy);
                }
        }
        public void PerformBKTree()
        {
            Console.WriteLine("Performation of BK Tree:");
            Perform(RootNode);
        }
        private void Perform(BKTreeNode NodePointer)
        {
            if (NodePointer.Children == null)
                return;

            foreach(var child in NodePointer.Children)
            {
                //Console.WriteLine($"{NodePointer.Word} {NodePointer.ChildrenAmount}");
                Console.WriteLine($"{NodePointer.Word}->{child.Value.Word} LD: {child.Key}");
            }
            foreach(var child in NodePointer.Children)
            {
                Perform(child.Value);
            }
        }
    }
}
