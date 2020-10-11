from locust import between, HttpUser, task

class RockPaperScissorsPlayer(HttpUser):
    wait_time = between(0.5, 1)
    host = "https://localhost:44388"

    def on_start(self):
        self.client.verify = False

    @task
    def choose_rock(self):
        self.client.post("/api/rockpaperscissors/validate/rock")
        self.client.post(
            json={
                    "isPlayerSelectionValid":True,
                    "playerChoice":1,
                    "computerChoice":0,
                    "gameResult":"",
                    "errorMessage":""
                },
            url="/api/rockpaperscissors/play")

    @task
    def choose_scissors(self):
        self.client.post("/api/rockpaperscissors/validate/paper")
        self.client.post(
            json={
                    "isPlayerSelectionValid":True,
                    "playerChoice":1,
                    "computerChoice":0,
                    "gameResult":"",
                    "errorMessage":""
                },
            url="/api/rockpaperscissors/play")

    @task
    def choose_paper(self):
        self.client.post("/api/rockpaperscissors/validate/scissors")
        self.client.post(
            json={
                    "isPlayerSelectionValid":True,
                    "playerChoice":1,
                    "computerChoice":0,
                    "gameResult":"",
                    "errorMessage":""
                },
            url="/api/rockpaperscissors/play")
    
    @task
    def choose_invalid(self):
        self.client.get("/api/rockpaperscissors/memoryLeak")