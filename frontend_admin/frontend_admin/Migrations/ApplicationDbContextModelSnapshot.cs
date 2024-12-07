﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using frontend_admin.Data;

#nullable disable

namespace frontend_admin.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("frontend_admin.Models.DetallesPracticas", b =>
                {
                    b.Property<int>("IdDetallePractica")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("iddetallepractica");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdDetallePractica"));

                    b.Property<int>("DisparoNumero")
                        .HasColumnType("integer")
                        .HasColumnName("disparonumero");

                    b.Property<int>("IdPractica")
                        .HasColumnType("integer")
                        .HasColumnName("idpractica");

                    b.Property<float>("Precision")
                        .HasColumnType("real")
                        .HasColumnName("precision");

                    b.Property<string>("RutaImagen")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("rutaimagen");

                    b.Property<string>("Ubicacion")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("ubicacion");

                    b.HasKey("IdDetallePractica");

                    b.HasIndex("IdPractica");

                    b.ToTable("detallespracticas");
                });

            modelBuilder.Entity("frontend_admin.Models.ExamenConfiguracion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("NumeroDisparos")
                        .HasColumnType("integer")
                        .HasColumnName("numerodisparos");

                    b.HasKey("Id");

                    b.ToTable("examenesconfiguracion");
                });

            modelBuilder.Entity("frontend_admin.Models.ExternalUser", b =>
                {
                    b.Property<string>("dni")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("dni");

                    b.Property<string>("face_descriptor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("face_descriptor");

                    b.Property<string>("image_path")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_path");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("dni");

                    b.ToTable("users", null, t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("frontend_admin.Models.ImpactosBala", b =>
                {
                    b.Property<int>("IdImpacto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idimpacto");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdImpacto"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("fecha");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("integer")
                        .HasColumnName("idusuario");

                    b.Property<float>("Precision")
                        .HasColumnType("real")
                        .HasColumnName("precision");

                    b.Property<string>("RutaImagen")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("rutaimagen");

                    b.Property<string>("Ubicacion")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("ubicacion");

                    b.HasKey("IdImpacto");

                    b.HasIndex("IdUsuario");

                    b.ToTable("impactosbala");
                });

            modelBuilder.Entity("frontend_admin.Models.Practicas", b =>
                {
                    b.Property<int>("IdPractica")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idpractica");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdPractica"));

                    b.Property<DateTime>("FechaPractica")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("fechapractica");

                    b.Property<int>("IdReserva")
                        .HasColumnType("integer")
                        .HasColumnName("idreserva");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("integer")
                        .HasColumnName("idusuario");

                    b.Property<float>("PrecisionPromedio")
                        .HasColumnType("real")
                        .HasColumnName("precisionpromedio");

                    b.Property<int>("TotalDisparos")
                        .HasColumnType("integer")
                        .HasColumnName("totaldisparos");

                    b.HasKey("IdPractica");

                    b.HasIndex("IdReserva");

                    b.HasIndex("IdUsuario");

                    b.ToTable("practicas");
                });

            modelBuilder.Entity("frontend_admin.Models.Reportes", b =>
                {
                    b.Property<int>("IdReporte")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idreporte");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdReporte"));

                    b.Property<string>("Detalles")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("detalles");

                    b.Property<DateTime>("FechaReporte")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("fechareporte");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("integer")
                        .HasColumnName("idusuario");

                    b.Property<float>("PromedioPrecision")
                        .HasColumnType("real")
                        .HasColumnName("promedioprecision");

                    b.Property<int>("TotalImpactos")
                        .HasColumnType("integer")
                        .HasColumnName("totalimpactos");

                    b.HasKey("IdReporte");

                    b.HasIndex("IdUsuario");

                    b.ToTable("reportes");
                });

            modelBuilder.Entity("frontend_admin.Models.Reservas", b =>
                {
                    b.Property<int>("IdReserva")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idreserva");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdReserva"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("estado");

                    b.Property<DateTime>("FechaReserva")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("fechareserva");

                    b.Property<int>("IdSala")
                        .HasColumnType("integer")
                        .HasColumnName("idsala");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("integer")
                        .HasColumnName("idusuario");

                    b.HasKey("IdReserva");

                    b.HasIndex("IdSala");

                    b.HasIndex("IdUsuario");

                    b.ToTable("reservas");
                });

            modelBuilder.Entity("frontend_admin.Models.SalaTiro", b =>
                {
                    b.Property<int>("IdSala")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idsala");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdSala"));

                    b.Property<bool>("Disponible")
                        .HasColumnType("boolean")
                        .HasColumnName("disponible");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("nombre");

                    b.HasKey("IdSala");

                    b.ToTable("salastiro");
                });

            modelBuilder.Entity("frontend_admin.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idusuario");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdUsuario"));

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("clave");

                    b.Property<bool>("Confirmado")
                        .HasColumnType("boolean")
                        .HasColumnName("confirmado");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("correo");

                    b.Property<string>("DNI")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("dni");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("nombre");

                    b.Property<bool>("Restablecer")
                        .HasColumnType("boolean")
                        .HasColumnName("restablecer");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("token");

                    b.HasKey("IdUsuario");

                    b.ToTable("usuarios");
                });

            modelBuilder.Entity("frontend_admin.Models.DetallesPracticas", b =>
                {
                    b.HasOne("frontend_admin.Models.Practicas", "Practica")
                        .WithMany("DetallesPracticas")
                        .HasForeignKey("IdPractica")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Practica");
                });

            modelBuilder.Entity("frontend_admin.Models.ImpactosBala", b =>
                {
                    b.HasOne("frontend_admin.Models.Usuario", "Usuario")
                        .WithMany("ImpactosBala")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("frontend_admin.Models.Practicas", b =>
                {
                    b.HasOne("frontend_admin.Models.Reservas", "Reserva")
                        .WithMany("Practicas")
                        .HasForeignKey("IdReserva")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("frontend_admin.Models.Usuario", "Usuario")
                        .WithMany("Practicas")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reserva");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("frontend_admin.Models.Reportes", b =>
                {
                    b.HasOne("frontend_admin.Models.Usuario", "Usuario")
                        .WithMany("Reportes")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("frontend_admin.Models.Reservas", b =>
                {
                    b.HasOne("frontend_admin.Models.SalaTiro", "Sala")
                        .WithMany("Reservas")
                        .HasForeignKey("IdSala")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("frontend_admin.Models.Usuario", "Usuario")
                        .WithMany("Reservas")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sala");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("frontend_admin.Models.Practicas", b =>
                {
                    b.Navigation("DetallesPracticas");
                });

            modelBuilder.Entity("frontend_admin.Models.Reservas", b =>
                {
                    b.Navigation("Practicas");
                });

            modelBuilder.Entity("frontend_admin.Models.SalaTiro", b =>
                {
                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("frontend_admin.Models.Usuario", b =>
                {
                    b.Navigation("ImpactosBala");

                    b.Navigation("Practicas");

                    b.Navigation("Reportes");

                    b.Navigation("Reservas");
                });
#pragma warning restore 612, 618
        }
    }
}
