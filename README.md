time-controller
===============

ClickOnce-deployed .NET applications don't have privileges to set the system date/time. This library works around that restriction by shelling out to an external assembly which requests elevated permissions.
