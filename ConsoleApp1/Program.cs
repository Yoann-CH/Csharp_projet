using System;
using System.Collections;

namespace projet
{
    class Compte
    {
        private int numero;
        private static int nombreDeCompte = 1;
        private Personne titulaireCompte;


        public Compte(int numero, Personne titulaireCompte)
        {
            this.numero = nombreDeCompte;
            this.titulaireCompte = titulaireCompte;
            nombreDeCompte++;
        }


        public int Numero
        {
            get { return numero; }
        }

        public Personne TitulaireCompte
        {
            get { return titulaireCompte; }
        }
    }

    class Personne
    {
        private string nom;
        private int numeroCarteIdentite;
        private string adresse;

        public Personne(string nom, int numeroCarteIdentite, string adresse)
        {
            this.nom = nom;
            this.numeroCarteIdentite = numeroCarteIdentite;
            this.adresse = adresse;
        }


    }

    class OperationBancaire
    {
        private int numero;
        private double montant;
        private string date;
        private string libelle;

        public OperationBancaire(int numero, float montant, string libelle)
        {
            this.numero = numero;
            this.montant = montant;
            this.date = DateTime.Now.ToString();
            this.libelle = libelle;
        }
    }
}
