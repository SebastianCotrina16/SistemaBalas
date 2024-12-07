from sqlalchemy import create_engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker

Base = declarative_base()

DATABASE_URL = "postgresql://SebastianCotrina16:OWQvYgs8o6Mm@ep-flat-block-17023118.us-east-2.aws.neon.tech/test?sslmode=require"

engine = create_engine(DATABASE_URL)
Session = sessionmaker(bind=engine)


def init_db():
    Base.metadata.create_all(engine)


def get_db_session():
    return Session()
