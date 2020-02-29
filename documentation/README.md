# Documentation
## Summary/What problem does it solve
**Category:** Replacement of Sitecore Meetup Website
In order to solve Sitecore problem with the future of Sitecore groups on Meetup, we decided to take the challenge and tried to create an MVP focusing in the key goals of what Sitecore groups community.
To speed up the development we used Sitecore Forms and SXA OOTB components as much as possible.

## Pre-requisites
Please make sure you have the following requirements:
+ Sitecore 9.3.0 rev. 003498 
+ Sitecore Experience Accelerator 9.3.0

## Installation
[Install Sitecore 9.3 rev 003498](https://dev.sitecore.net/~/media/A1BC9FD8B20841959EF5275A3C97A8F9.ashx)
Install Sitecore Experience Accelerator 9.3.0 (included in the setup above)

## Configuration
Everything should be included in the provided package (ADICIONAR LINK) and steps above.

1- First install the two packages provided. This packages are to be used in a clean instance of Sitecore 9.3 with SXA 9.3 pre-installed. First install the Package 1 and then the Package 2
2- Run the SQL script (ADICIONAR LINK PARA O SCRIPT) to create the additional database a table that will be used to create the module items.
3- Add the new connectionstring to the connectionstring.config.
4- Deploy the code of the repository.
 
## Technical breakdown
The key goals of this community are share events and group people according to their interests and/or localization.
All the presentation is controlled by SXA variants using rules or SXA component settings. Some of the components are using personalization according to authentication status.
To accomplish these goals, we developed the following functionalities:
1. Authentication
2. Create groups
3. Create events
4. Show a calendar with the events related to a group or a user

### Authentication
Composed by an SXA login component and Sitecore Forms for register users.
Because is just a proof of concept, we used Sitecore users in the domain “mydomain”.
Login and Logout components are controlled through personalization rules to display the respective component if the user is logged in or not.

### Create groups
As an authenticated user, it is possible to create a Group and be the Owner.
This can be done through a page with a Sitecore Form that will create this Group as a page under Groups.
The decision of creating a group as a page was based on trying to take the most out of SXA capabilities like search, variants and OOTB components and personalization.

### Create events
As an authenticated user, it is possible to create an Event and be the Organizer.
This can be done through a page with a Sitecore Form that will create this Event as a page under a Group.
These events are using SXA Calendar Events, and this was also one of the reasons of choosing this way of structuring. Once again, this decision was based on trying to take the most out of SXA.

### Calendar
The calendar was done by using an extension of SXA Calendar in order to be possible to see all the child events of a Group.

## Usage
### Register
1. Navigate to any page
2. On the header, click on the register link
3. Fill the form and click register
4. You will be redirected to a success page

### Authentication
1. Navigate to any page
2. Fill the login form on the header, using mydomain/[username]
3. Press Log In
4. To logout, there is a Logout button in the place of Login form 

### Home Page
1. Navigate to the home page
2. A list of available events will be displayed, click on one to follow to the respective Event page

### Event Page
1. Go to an event page
2. You can navigate to each member profile and see the details of the event

### Group Page
1. Go to a group page
2. Here you can see the details of the group, list of events and members, the organizer and the calendar
3. At the bottom of the page, if you are authenticated, you can see the form to create the Event.

## Video
Direct link to the video:  

Please provide a video highlighing your Hackathon module submission and provide a link to the video. Either a [direct link](https://www.youtube.com/watch?v=EpNhxW4pNKk) to the video, upload it to this documentation folder or maybe upload it to Youtube...

[![Sitecore Hackathon Video Embedding Alt Text](https://img.youtube.com/vi/EpNhxW4pNKk/0.jpg)](https://www.youtube.com/watch?v=EpNhxW4pNKk)