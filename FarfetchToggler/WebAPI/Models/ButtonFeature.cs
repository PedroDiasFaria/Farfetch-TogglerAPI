namespace WebAPI.Models
{
    public class ButtonFeature : Feature
    {
        /*  In case of A/B testing, we decide which users can use the service
         *  So, we create a list of userTypes that can use it, and concatenate them as a string
         *  admin;premium;normal
         */
        public ButtonFeature(string name, string type, bool featureFlag, string permissions) : base(name, type, featureFlag, permissions)
        {
        }

        public ButtonFeature() : base()
        {

        }
    }

    public class BlueButton : ButtonFeature
    {
        /****************/
        //If featureflag = true, add the list
        //important if we want to initiate with a custom list of permissions (when updating/creating a feature in the api)
        /*
         * Default constructor
         * */
        public BlueButton(string name) : base(name, "BlueButton", true, "admin;normal;premium")
        {//constructor: receiving true or false difers on the users
        }

        /*
         * Customized constructor 
         * */
        public BlueButton(string name, bool featureFlag, string permissions) : base(name, "BlueButton", featureFlag, permissions)
        {//constructor: receiving true or false difers on the users
        }

        public BlueButton() : base()
        {

        }

        /*
         * If 'On': all can use
         *      'Off': only Premium and Admin can use
         * 
         */
        public override bool Toggle()
        {
            SetPermission(base.Toggle(), "normal");
            return FeatureFlag;
        }
    }

    public class GreenButton : ButtonFeature
    {
        public GreenButton(string name) : base(name, "GreenButton", true, "admin;premium")
        {
        }

        public GreenButton(string name, bool featureFlag, string permissions) : base(name, "GreenButton", featureFlag, permissions)
        {

        }

        public GreenButton() : base()
        {

        }

        /*
         * If 'On': only Premium and Admin can use
         *      'Off': only Admin can use
         * 
         */
        public override bool Toggle()
        {
            SetPermission(base.Toggle(), "premium");
            return FeatureFlag;
        }
    }

    public class RedButton : ButtonFeature
    {
        public RedButton(string name) : base(name, "RedButton", true, "admin;normal")
        {
        }

        public RedButton(string name, bool featureFlag, string permissions) : base(name, "RedButton", featureFlag, permissions)
        {

        }

        public RedButton() : base()
        {

        }

        /*
        * If 'On': only Normal and Admin can use
        *      'Off': only Admin can use
        * 
        */
        public override bool Toggle()
        {
            SetPermission(base.Toggle(), "normal");
            return FeatureFlag;
        }
    }
}