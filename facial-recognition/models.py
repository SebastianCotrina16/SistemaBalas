from sqlalchemy import Column, Integer, String, create_engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker

Base = declarative_base()

class User(Base):
    __tablename__ = 'users'
    dni = Column(String, primary_key=True)
    name = Column(String)
    face_descriptor = Column(String)
    image_path = Column(String)
    
    def __repr__(self):
        return f"User(dni={self.dni}, name={self.name}, face_descriptor={self.face_descriptor}, image_path={self.image_path})"

DATABASE_URL = "sqlite:///users.db"
engine = create_engine(DATABASE_URL, connect_args={"check_same_thread": False})
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base.metadata.create_all(bind=engine)
