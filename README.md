# Attendance Log
Web API to log attendance of users into a SQL Server database.  Can be used for a variety of application i.e., schools, conferences, offices etc.   Tables below summarise how to input data into Location, Log, and User categories.

## Location
Description  | URL  | Input Required  | Input Type
------------- | -------------  | ------------- | -------------
Get a list of all locations  | /api/Location/all   | n/a  | n/a
Get a location record by Id  | /api/Location/{locationId}   | Unique Id  | Number in Path
Search for records by location  | /api/Location/search   | Company, Building, City, Country  | Text in Query
Create a new location  | /api/Location/create   | Company, Building, City, Country  | Text in Body
Update a location  | /api/Location/edit/{locationId}   | Unique Id, Company, Building, City, Country  | Number in Path and Body. Text in Body
Delete a location  | /api/Location/delete/{locationId}   | Unique Id  | Number in Path

## Log
Description  | URL  | Input Required  | Input Type
------------- | -------------  | ------------- | -------------
Get a list of all logs  | /api/Log/all   | n/a  | n/a
Get a log record by Id  | /api/Log/{logId}   | Unique Id  | Number in Path
Get a list of logs by user Id  | /api/Log/user/{userId}   | Unique Id  | GUID in Path
Search for log records by date and attendance  | /api/Log/search   | Date and Attendance  | Date and Boolean in Query.  For example, 2024-11-26 and true
Search for log records by user names  | /api/Log/search/user   | Forename and Surname  | Text in Query.
Search for log records by location  | /api/Log/search/location   | Company, Building, City, Country  | Text in Query
Create a new log  | /api/Log/create   | Unique User Id, Present, Unique Location Id  | GUID, Boolean, Number in Body

## User
Description  | URL  | Input Required  | Input Type
------------- | -------------  | ------------- | -------------
Get a list of all users  | /api/User/all   | n/a  | n/a
Get a user record by Id  | /api/User/{userId}   | Unique Id  | GUID in Path
Search for records by user  | /api/User/search   | Forename and Surname  | Text in Query
Create a new user  | /api/User/create   | Forename and Surname  | Text in Body
Update a user  | /api/User/edit/{userId}   | Unique Id, Forename and Surname  | GUID in Path and Body. Text in Body
Delete a user  | /api/User/delete/{UserId}   | Unique Id  | GUID in Path

