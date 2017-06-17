using System.Collections;

namespace LCARS.CoreUi.UiElements.Tabbing
{
    public class LcarsTabPageCollection : CollectionBase
    {
        //This class keeps track of all the tabs on the LcarsTabControl.  It's a fairly simple class,
        //as the 'CollectionBase' class does all of the heavy lifting.

        //The LcarsTabControl the current instance of LcarsTabCollection is running inside of
        private LcarsTabControl Parent;
        internal LcarsTabPageCollection(LcarsTabControl Control)
        {
            //When an LcarsTabControl creates it's TabPages collection, it passes itself as the single
            //parameter so we can tell what LcarsTabControl goes with the collection.
            Parent = Control;
        }

        public LcarsTabPage this[int Index]
        {
            //Returns whatever tabpage is at the given index.
            get { return (LcarsTabPage)List[Index]; }
        }

        public bool Contains(LcarsTabPage tab)
        {
            //Returns 'true' if the tab is in the collection and 'false' if it is not.
            return List.Contains(tab);
        }

        public int Add(LcarsTabPage tab)
        {
            //Adds the supplied tab to the tab collection and returns it's new index
            int i = 0;
            i = List.Add(tab);
            Parent.TabPagesChanged();
            return i;
        }

        public int IndexOf(LcarsTabPage tab)
        {
            //Returns the index of the given tab in the collection
            return List.IndexOf(tab);
        }

        public void Remove(LcarsTabPage Tab)
        {
            //Removes the given tab from the collection.  I know... duh. But comments are necessary evils.
            List.Remove(Tab);
            Tab = null;
            Parent.TabPagesChanged();
        }

        public void MoveDown(LcarsTabPage tab)
        {
            int index = List.IndexOf(tab);
            if (index < Count - 1)
            {
                List[index] = List[index + 1];
                List[index + 1] = tab;
            }
            Parent.TabPagesChanged();
        }

        public void MoveUp(LcarsTabPage tab)
        {
            int index = List.IndexOf(tab);
            if (index > 0)
            {
                List[index] = List[index - 1];
                List[index - 1] = tab;
            }
            Parent.TabPagesChanged();
        }
    }
}
