# Unity-GPT-Chat-Example

Simple Unity project + Python backend server setup to get started using GPT from OpenAI.

You would still need to implement user authentification for usage outside of a dev environment.

## Setup Server

- Add your OpenAI API key in `./PythonServer/.env`

```.evn
# ./PythonServer/.env

OPENAI_API_KEY=YOUR_API_KEY
```

- [Recommend to use a virtual environment]([https://](https://docs.python.org/3/library/venv.html))
- install the requirements `pip install -r requirements.txt`
- run server with `python NLP_server.py`

## Setup Unity

- Import project in Unity `>= 2021.3.8f1`
- Open the Scene `Scenes/NLP API Demo.unity`
- Run the app