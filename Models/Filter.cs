namespace Equinox.Models
{
    public class Filter
    {
        public Filter(string filtered)
        {
            filter = filtered ?? "all-all";
            string[] filteredSplit = filter.Split('-');
            ClubID = filteredSplit[0];
            ClassCategoryID = filteredSplit[1];
        }
        public string filter { get; }
        public string ClubID { get; }
        public string ClassCategoryID { get; }

        public bool HasClubs => ClubID.ToLower() != "all";
        public bool HasClassCategory => ClassCategoryID.ToString().ToLower() != "all";
    }
}
