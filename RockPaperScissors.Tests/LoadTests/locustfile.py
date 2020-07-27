from locust import between, HttpUser, task

class RockPaperScissorsPlayer(HttpUser):
    wait_time = between(0.5, 1)
    host = "http://localhost:62495"

    def on_start(self):
        self.client.verify = False

    @task
    def choose_rock(self):
        self.client.post("/api/rockpaperscissors/rock")

    @task
    def choose_scissors(self):
        self.client.post("/api/rockpaperscissors/scissors")

    @task
    def choose_paper(self):
        self.client.post("/api/rockpaperscissors/paper")