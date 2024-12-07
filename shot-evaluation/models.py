from sqlalchemy import Column, Integer, String, Float, DateTime, ForeignKey, Boolean
from sqlalchemy.orm import relationship
from sqlalchemy.ext.declarative import declarative_base
import datetime

Base = declarative_base()


class Usuario(Base):
    __tablename__ = 'usuarios'

    idusuario = Column(Integer, primary_key=True, autoincrement=True)
    nombre = Column(String(50), nullable=False)
    correo = Column(String(50), nullable=False, unique=True)
    clave = Column(String(200), nullable=False)
    restablecer = Column(Boolean, default=False)
    confirmado = Column(Boolean, default=False)
    token = Column(String(200))
    dni = Column(String(20), nullable=False, unique=True)

    impactoses = relationship("ImpactosBala", back_populates="usuario")
    reportes = relationship("Reportes", back_populates="usuario")


class ImpactosBala(Base):
    __tablename__ = 'impactosbala'

    idimpacto = Column(Integer, primary_key=True, autoincrement=True)
    idusuario = Column(Integer, ForeignKey(
        'usuarios.idusuario'), nullable=False)
    fecha = Column(DateTime, default=datetime.datetime.utcnow)
    ubicacion = Column(String(255))
    precision = Column(Float)
    rutaimagen = Column(String(255))

    usuario = relationship("Usuario", back_populates="impactoses")


class Reportes(Base):
    __tablename__ = 'reportes'

    idreporte = Column(Integer, primary_key=True, autoincrement=True)
    idusuario = Column(Integer, ForeignKey(
        'usuarios.idusuario'), nullable=False)
    fechareporte = Column(DateTime, default=datetime.datetime.utcnow)
    totalimpactos = Column(Integer, nullable=False)
    promedioprecision = Column(Float, nullable=False)
    detalles = Column(String)

    usuario = relationship("Usuario", back_populates="reportes")
