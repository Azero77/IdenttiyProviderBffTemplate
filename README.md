# README

## Project Template For Authorization and Authentication using BFF Pattern

### This solution contains 5 projects:
1-**App.Identtiy** -> for authorizing and authenticating the user and get the access token and identity token
1-**App.Bff** -> stores this token in sessions
1-**App.Api** -> have the remote api to add authorizing to
1-**App.Client** -> stores encrypted cookies that has an id to the session 
1-**App.Shared** -> contains some basic configuration and shared models between the projects
