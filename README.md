# Appointment Booking Challenge

## Notes to Reviewers

- Please correct 'Requirements' section in the requirements.pdf file. Expected output says that the format of `start_date`
field should `2024-05-03T12:00:00.00Z`, two zeros before `Z`, which is not usual, however the e2e tests expect `start_date` 
to be `2024-05-03T12:00:00.000Z`, three zeros before `Z`, which is common format for UTC dates. Formally speaking, 
the document must be a source of truth for me and I must implement the solution according to it but in this case the test 
suite will fail.

- In the requirements.pdf following is mentioned: 'We might ALSO run additional tests, such as loading thousands of records 
in the database to assert the application is performant enough' and 'Efficiency and performance of the api endpoint'. 
But because there is no definition of what is 'performant enough' for the that particular case, I chose to implement the 
solution in the most straightforward and simple for understanding way. Additional techniques and optimization can be 
implemented but only with clear requirements regarding expected performance.