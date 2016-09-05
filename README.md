# WeatherTest

Thank you for taking the time to review my submission

### Requirements
Your application/website should:
1. Display the aggregated (average) result, both temperature and wind speed, from any APIs it has queried
2. Allow the user to choose which measurement unit they want results displayed in. Wind should be MPH or KPH and temperature should be Celsius or Fahrenheit
3. Allow for more APIs to be easily added in the future
4. Allow for other units of measure to be easily added in the future
5. Gracefully handle one or more of the APIs being down or slow to respond

It should pass the following tests:
6.	Given temperatures of 10c from bbc and 68f from accuweather when searching then display either 15c or 59f depending on what the user has chosen.
7.	Given wind speeds of 8kph from bbc and 10mph from accuweather when searching then display either 12kph or 7.5mph depending on what the user has chosen.

### Implementation Notes
1. Results are queried and then the average is taken and reported back to the user.
2. The WeatherService database table records the units reported by the service and performs conversion where necessary before adding to the pre-averaged results.
3. Service URLs are stored in the database with pointers in the string to switch out the location.
4. Units are stored in the database along with conversion formulae to convert to other units.
5. Exceptions (including timeout) result in a zero result being returned. In a real situation this would be expanded to specify timeouts.
6&7. Unit tests verify these conditions/results.

3&4 are demonstrated through the "Manage Options"/"Manage Sources" dialog which update the database with a reference to the Yahoo weather service and add "Kelvin" and "Feet/Second" units.

