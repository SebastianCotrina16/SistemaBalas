from app import app

def before_scenario(context, scenario):
    """Este hook se ejecuta antes de cada escenario"""
    context.client = app.test_client()
    context.client.testing = True
