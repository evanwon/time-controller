TimeController for .NET
===============

ClickOnce-deployed .NET applications don't have privileges to set the system date/time. This library works around that restriction by shelling out to an external assembly which requests elevated permissions.

**Heads up!** This library is 100% experimental at the moment and hasn't been validated in production yet.