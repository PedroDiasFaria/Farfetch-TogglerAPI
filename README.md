# Farfetch-TogglerAPI
Farfetch Technical Exercise : REST Web Api w/ Feature Toggling

## Important files:

WebAPI	/Controllers
		/Models

MVC		/Models
		/Views
		/Controllers

## Implemented:

- [x] Feature Toggles with Hierarchy:
  - Have 3 test Features called `BlueButton`, `GreenButton` and `RedButton`, which:
    - Are subclasses of `ButtonFeature`, a subclass of `Feature`
    ![Custom Toggles](/assets/FeaturesToggleCustom.png)
- [x] REST API for a simple creation / modification/ deletion of new Features and Users
	![Main View](/assets/IndexView.png)
- [x] Features Toggling that can be checked if a Feature is available to a particular type of Users
  ![Feature Toggles On](/assets/FeaturesToggleOn.png)
  ![Feature Toggles Off](/assets/FeaturesToggleOff.png)
  - Following the Exercise example:
  
  ![Default Toggle Options](/assets/DefaultToggleOptions.png)

## Not Implemented:

- [ ] Registration / Login:
  - Although there is no Authentication system in the Application, users only have access to their own features accordingly their userType and the Feature permissions:
  	![Admin Features](/assets/AdminFeatures.png)
  	![Normal User Features](/assets/NormalFeatures.png)
  	![Premium User Features](/assets/PremiumFeatures.png)


## Running

The project is divided in 2 parts:
  - A webAPI, with all the server side operations
  - A MVC client, to simulate an Admin pannel with a UI to easily test all the aforementioned features

Simply download the source code, build and run the solution. A pre-seeded Database is available with some example Features and Users