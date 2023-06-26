import matplotlib.pyplot as plt
import json

with open ('data.json', 'r') as file:
    data = json.load(file)

blueWin = 0
redWin = 0

for key, value in data['Version6_25'].items():
    if value.get('winner') == "Player1":
        blueWin+=1
    else:
        redWin+=1
        
labels = 'Player1-BlueCar', 'Player2-RedCar'
colors = ['blue', 'red']
sizes = [blueWin, redWin]


plt.legend('', frameon=False)
_, _, autotexts = plt.pie(sizes, colors=colors, autopct='%1.1f%%')
plt.axis('equal')
plt.title("Number of Wins by Vehicle Type", bbox={'facecolor':'0.8', 'pad':5})
legend_handles = [plt.Rectangle((0, 0), 1, 1, color=color) for color in colors]
legend = plt.legend(legend_handles, labels, loc='lower left')


plt.show()
