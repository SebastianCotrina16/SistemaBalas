from behave import given, when, then
from app import app, SessionLocal
from models import User
from flask import json

@given('I have a user with DNI "{dni}" and name "{name}"')
def step_given_user_with_dni_and_name(context, dni, name):
    context.dni = dni
    context.name = name

@when('I upload the registration image "{image_path}"')
def step_when_upload_registration_image(context, image_path):
    with open(image_path, 'rb') as img:
        context.response = context.client.post('/register', data={
            'dni': context.dni,
            'name': context.name,
            'image': img
        })
        context.response_data = json.loads(context.response.data)

@then('the user should be successfully registered')
def step_then_user_should_be_successfully_registered(context):
    assert 'success' in context.response_data, "No 'success' key in response"
    assert context.response_data['success'] == True

@then('the user with DNI "{dni}" should exist in the database')
def step_then_user_should_exist_in_database(context, dni):
    session = SessionLocal()
    try:
        user = session.query(User).filter_by(dni=dni).first()
        assert user is not None, f"User with DNI {dni} not found"
    finally:
        session.close()

@then('the registered user name should be "{name}"')
def step_then_registered_user_name_should_be(context, name):
    session = SessionLocal()
    try:
        user = session.query(User).filter_by(dni=context.dni).first()
        assert user is not None, f"User with DNI {context.dni} not found"
        assert user.name == name, f"Expected user name to be {name}, got {user.name}"
    finally:
        session.close()

