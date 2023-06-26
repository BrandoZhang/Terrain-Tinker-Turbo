import matplotlib.pyplot as plt
'
with open('data.json', 'r') as file:
    data = json.load(file)
'
# JSON data
data = {
    "WinRoundCount": {
        "Player1": 3,
        "Player2": 2
    }
}

# Extract player names and round counts
players = list(data["WinRoundCount"].keys())
round_counts = list(data["WinRoundCount"].values())

# Custom legend labels
legend_labels = [f'{player}-{"BlueCar" if player == "Player1" else "RedCar"}' for player in players]

# Custom colors
colors = ['blue', 'red']

# Create pie chart
plt.legend('', frameon=False)
_, _, autotexts = plt.pie(round_counts, colors=colors, autopct='%1.1f%%')
plt.axis('equal')  # Equal aspect ratio ensures that pie is drawn as a circle
plt.title("Number of Wins by Vehicle Type", bbox={'facecolor':'0.8', 'pad':5})

# Create separate legend box
legend_handles = [plt.Rectangle((0, 0), 1, 1, color=color) for color in colors]
legend = plt.legend(legend_handles, legend_labels, loc='lower left')


plt.show()
