Destiny Public Events Master (Destiny PEM)

Software that polls destinypublicevents.com to find the best order to visit public events, maximizing events per hour.

Destiny PEM takes many factors into travel time - distance between events on the same planet, time to load the planet, and time to complete the events themselves.

HOWEVER - After preliminary testing I found that most configurations ended up reflecting a simple pattern - "pick the first >0 event timer on the page" as the optimal general solution... At the very least, this was a fun project and I enjoyed the software modeling aspect of it. :)

Then again, the calculations were done with inaccurate test data, so maybe there's something more to this...

---

The destinypublicevents.com polling is probably out of date, but the data model shouldn't need to change - only the adapter.


Configuration of the system parameters can be done through the .txt files found in Destiny-PEM.UI

- event-completion-times.txt
- locations-list.txt
- travel-times.txt
