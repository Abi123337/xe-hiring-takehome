1. Objective
The application currently displays live exchange rates using the XE Currency Data API.
The new requirement is to allow users to create alerts such as:
Notify me when GBP/CAD goes above 1.84.
The system should:
•	Create a rate alert.
•	Store the alert.
•	Retrieve a user's alerts.
•	Delete an alert.
•	Periodically check the latest exchange rate.
•	Mark an alert as triggered when its condition is met.
•	Display the triggered status in the Vue frontend.

2. High Level Architecture
                       RATE ALERT - HIGH LEVEL ARCHITECTURE
                    ====================================


                              ┌─────────────────────┐
                              │       Vue 3 UI      │
                              │                     │
                              │ Create Alert        │
                              │ List Alerts         │
                              │ Delete Alert        │
                              │ Show Triggered      │
                              └──────────┬──────────┘
                                         │
                                         │ HTTP / REST
                                         ▼
                              ┌─────────────────────┐
                              │  AlertsController   │
                              │                     │
                              │ POST /api/alerts    │
                              │ GET  /api/alerts    │
                              │ DELETE /api/alerts/ │
                              └──────────┬──────────┘
                                         │
                                         ▼
                              ┌─────────────────────┐
                              │    AlertService     │
                              │                     │
                              │ • Validation        │
                              │ • Business Rules    │
                              │ • Alert Evaluation  │
                              └───────┬──────┬──────┘
                                      │      │
                         ┌────────────┘      └────────────┐
                         ▼                                ▼
              ┌─────────────────────┐          ┌─────────────────────┐
              │  Alert Repository   │          │   XE Rate Service   │
              │                     │          │                     │
              │ • Store Alerts      │          │ • Get Current Rate  │
              │ • Get Alerts        │          │ • XE API Integration│
              │ • Update Alerts     │          │                     │
              │ • Delete Alerts     │          │                     │
              └──────────┬──────────┘          └──────────┬──────────┘
                         │                                │
                         ▼                                ▼
              ┌─────────────────────┐          ┌─────────────────────┐
              │ In-Memory Repository│          │   XE Currency Data │
              │                     │          │        API          │
              │ ConcurrentDictionary│          │                     │
              └─────────────────────┘          └─────────────────────┘



                     BACKGROUND ALERT EVALUATION
                     ============================


                    ┌───────────────────────────────┐
                    │ AlertEvaluation               │
                    │ BackgroundService             │
                    │                               │
                    │ Runs every 30 seconds         │
                    └───────────────┬───────────────┘
                                    │
                                    │
                                    ▼
                         ┌─────────────────────────┐
                         │      AlertService       │
                         │                         │
                         │ EvaluateAlertsAsync()   │
                         └────────────┬────────────┘
                                      │
                                      ▼
                         ┌─────────────────────────┐
                         │ Get Active Alerts       │
                         │                         │
                         │ Triggered = false       │
                         └────────────┬────────────┘
                                      │
                                      ▼
                         ┌─────────────────────────┐
                         │ Group by Currency Pair  │
                         │                         │
                         │ GBP/CAD                 │
                         │ USD/EUR                 │
                         │ GBP/USD                 │
                         └────────────┬────────────┘
                                      │
                                      ▼
                         ┌─────────────────────────┐
                         │    XE Rate Service      │
                         │                         │
                         │ Get latest rate         │
                         └────────────┬────────────┘
                                      │
                                      ▼
                         ┌─────────────────────────┐
                         │      XE Currency API    │
                         │                         │
                         │ Current GBP/CAD = 1.85  │
                         └────────────┬────────────┘
                                      │
                                      ▼
                         ┌─────────────────────────┐
                         │ Compare Rate & Threshold│
                         │                         │
                         │ Above: Rate >= Threshold│
                         │ Below: Rate <= Threshold│
                         └────────────┬────────────┘
                                      │
                         ┌────────────┴────────────┐
                         │                         │
                         ▼                         ▼
                 ┌─────────────────┐       ┌─────────────────┐
                 │ Condition NOT   │       │ Condition MET   │
                 │      MET        │       │                 │
                 │                 │       │ Triggered=true  │
                 │ Keep Waiting    │       │ TriggeredAt=UTC │
                 └─────────────────┘       └────────┬────────┘
                                                    │
                                                    ▼
                                           ┌─────────────────┐
                                           │ Update Alert    │
                                           │ Repository      │
                                           └────────┬────────┘
                                                    │
                                                    ▼
                                           ┌─────────────────┐
                                           │ Vue refreshes   │
                                           │ alert status    │
                                           └────────┬────────┘
                                                    │
                                                    ▼
                                                🔔 Triggered

   
3. I spent around 3 hrs to design and come up with the project structure with a base code. The code is not entirely tested, and I could cover only minimal unit test cases. Next steps would be to add more business rules or validations, unit test cases, generic currency pairs and UI logic to call the alert APIs.
4. I primarily used Chat GPT for the design diagrams and get some initial reviews on my design.
