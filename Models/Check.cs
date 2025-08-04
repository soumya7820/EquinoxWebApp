namespace Equinox.Models
{
    public static class Check
    {
        public static string PhoneNumberExists(EquinoxDbContext ctx, string phonenumber)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(phonenumber)) {
                var customer = ctx.User.FirstOrDefault(
                    c => c.PhoneNumber.ToLower() == phonenumber.ToLower());
                if (customer != null) 
                    msg = $"Phone Number {phonenumber} already in use.";
            }
            return msg;
        }
    }
}