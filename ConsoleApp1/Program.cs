using System;
using System.Collections;

namespace projet
{
    abstract class Compte
    {
        protected int numero;
        protected static int nombreDeCompte = 1;
        protected Personne titulaireCompte;
        protected List<OperationBancaire> operationBancaires;


        public Compte(Personne titulaireCompte)
        {
            numero = nombreDeCompte;
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

        public List<OperationBancaire> OperationBancaires
        {
            get { return operationBancaires; }
        }

        public abstract void Crediter(double credit);

        public abstract void Debiter(double debit);

        public abstract string toString();

    }

    class Courant:Compte{
        private double solde;
        private double decouvertMaximalAutorise;
        private double debitMaximalAutorise;
        private bool decouvert;

        public Courant(Personne titulaireCompte, double solde, double decouvertMaximalAutorise, double debitMaximalAutorise) : base(titulaireCompte)
        {
            if(solde < decouvertMaximalAutorise)
            {
                Console.WriteLine("Le solde ne peut pas être en dessous du découvert maximal autorisé !");
                this.solde = 0;
            }
            else
            {
                if(solde < 0)
                {
                    decouvert = true;
                }
                else
                {
                    decouvert = false;
                }
                this.solde = solde;
            }
            this.decouvertMaximalAutorise = decouvertMaximalAutorise;
            this.debitMaximalAutorise = debitMaximalAutorise;
        }

        public bool Decouvert
        {
            get { return decouvert; }
        }

        public double DecouvertMaximalAutorise
        {
            get { return decouvertMaximalAutorise; }
            set { decouvertMaximalAutorise = value; }
        }

        public double DebitMaximalAutorise
        {
            get { return debitMaximalAutorise; }
            set { debitMaximalAutorise = value; }
        }

        public override void Crediter(double credit)
        {

            solde += credit;
            if (solde < 0)
            {
                decouvert = true;
            }
            else
            {
                decouvert = false;
            }
            OperationBancaire o = new OperationBancaire(credit, "compte crédité");
            OperationBancaires.Add(o);
        }

        public override void Debiter(double debit)
        {
            if(debit > debitMaximalAutorise)
            {
                Console.WriteLine("Le montant débité ne peut pas être superieur au débit maximal autorisé !");
            }
            else  if(solde - debit < decouvertMaximalAutorise)
            {
                Console.WriteLine("Le solde ne peut pas être inférieur au découvert maximal autorisé !");
            }
            else
            {
                solde -= debit;
                if (solde < 0)
                {
                    decouvert = true;
                }
                else
                {
                    decouvert = false;
                }
                OperationBancaire o = new OperationBancaire(debit, "compte débité");
                OperationBancaires.Add(o);
            }
        }

        public override string toString()
        {
            string text = "";
            text += "nom du client : " + titulaireCompte.Nom + " numero carte d'identité : " + titulaireCompte.NumeroCarteIdentite + " adresse du client " + titulaireCompte.Adresse + "\n"+"numero de compte : " + numero + "solde du compte : " + solde + " découvert maximal autorisé : " + decouvertMaximalAutorise + " débit maximal autorisé : " + debitMaximalAutorise + "\n";
            if (decouvert)
            {
                text += "Le compte est à découvert.\n";
            }
            else
            {
                text += "Le compte n'est pas à découvert.\n";
            }
            int cinqDerniereOperation = 0;
            foreach(OperationBancaire o in operationBancaires)
            {
                if(cinqDerniereOperation == 4)
                {
                    break;
                }
                else
                {
                    text += "montant de la transaction : " + o.Montant + " Date de l'opération : " + o.Date + " information de l'opération : " + o.Libelle+"\n";
                    cinqDerniereOperation++;
                }
            }
            return text;
        }
    }

    class Epargne : Compte
    {
        private double solde;
        private double interet;

        public Epargne(Personne titulaireCompte, double solde, double interet) : base(titulaireCompte)
        {
            if(solde < 0)
            {
                Console.WriteLine("Le solde ne peut pas être négatif pour un compte d'épargne");
                this.solde = 0;
            }
            else
            {
                this.solde = solde;
            }
            this.interet = interet;
        }

        public override void Crediter(double credit)
        {
            solde += credit;
            OperationBancaire o = new OperationBancaire(credit, "compte crédité");
            OperationBancaires.Add(o);
        }

        public override void Debiter(double debit)
        { 
            if (solde - debit < 0)
            {
                Console.WriteLine("Le solde ne peut pas être inférieur à 0 !");
            }
            else
            {
                solde -= debit;
                OperationBancaire o = new OperationBancaire(debit, "compte débité");
                OperationBancaires.Add(o);
            }
        }

        public override string toString()
        {
            string text = "";
            text +=  "nom du client : " + titulaireCompte.Nom + " numero carte d'identité : " + titulaireCompte.NumeroCarteIdentite + " adresse du client " + titulaireCompte.Adresse + "\n" + "numero de compte : "+ numero +"solde du compte : " + solde + " intéret : " + interet + "\n";
            int cinqDerniereOperation = 0;
            foreach (OperationBancaire o in operationBancaires)
            {
                if (cinqDerniereOperation == 4)
                {
                    break;
                }
                else
                {
                    text += "montant de la transaction : " + o.Montant + " Date de l'opération : " + o.Date + " information de l'opération : " + o.Libelle + "\n";
                    cinqDerniereOperation++;
                }
            }
            return text;
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

        public string Nom
        {
            get { return nom; }
        }

        public int NumeroCarteIdentite
        {
            get { return numeroCarteIdentite; }
        }

        public string Adresse
        {
            get { return adresse; }
        }

    }

    class OperationBancaire
    {
        private int numero;
        private int nombreDeOperation = 1;
        private double montant;
        private string date;
        private string libelle;

        public OperationBancaire(double montant, string libelle)
        {
            numero = nombreDeOperation;
            nombreDeOperation++;
            this.montant = montant;
            date = DateTime.Now.ToString();
            this.libelle = libelle;
        }

        public double Montant
        {
            get { return montant; }
        }

        public string Date
        {
            get { return date; }
        }

        public string Libelle
        {
            get { return libelle; }
        }
    }

    class Banque
    {
        private string nom;
        private List<Compte> comptes;
        private List<Personne> clients;

        public Banque(string nom)
        {
            this.nom = nom;
        }

        public string Nom
        {
            get { return nom; }
        }

        public List<Compte> Comptes
        {
            get { return comptes; }
        }

        public List <Personne> Clients
        {
            get { return clients; }
        }

        public void CreerClient()
        {
            Console.WriteLine("Nom du client :");
            string nomClient = Console.ReadLine();
            Console.WriteLine("Adresse du client :");
            string adresseClient = Console.ReadLine();
            int result;
            int SaisieNombre()
            {
                while (!int.TryParse(Console.ReadLine(), out result)) ;
                return result;
            }
            int numeroCarteIdentiteClient = SaisieNombre();
            Personne client = new Personne(nomClient, numeroCarteIdentiteClient, adresseClient);
            clients.Add(client);
        }

        public void SupprimerClient()
        {
            Console.WriteLine("Nom du client à supprimer :");
            string nomClient = Console.ReadLine();
            foreach(Personne client in clients)
            {
                if(client.Nom == nomClient)
                {
                    clients.Remove(client);
                    return;
                }
            }
            Console.WriteLine("le nom renseigné ne correspond pas à un client de la banque !");
        }

        public void ajouterCompte()
        {
            Console.WriteLine("Compte épargne (0) ou compte courant (1) ?");
            int result;
            int SaisieNombre()
            {
                while (!int.TryParse(Console.ReadLine(), out result)) ;
                return result;
            }
            int typeCompte = SaisieNombre();
            if(typeCompte == 0)
            {
                Console.WriteLine("solde du compte : ");
                double resultDouble;
                 double SaisieNombreDouble()
                {
                    while (!double.TryParse(Console.ReadLine(), out resultDouble)) ;
                    return resultDouble;
                }
                double soldeCompte = SaisieNombreDouble();
                Console.WriteLine("intéret du compte : ");
                double interet = SaisieNombreDouble();
                Console.WriteLine("nom du titulaire : ");
                string nomTitulaire = Console.ReadLine();
                bool verifTitulaire = false;
                foreach (Personne client in clients)
                {
                    if (client.Nom == nomTitulaire)
                    {
                        Personne p = client;
                        verifTitulaire = true;
                        Epargne e = new Epargne(p, soldeCompte, interet);
                        comptes.Add(e);
                    }
                }
                if (!verifTitulaire)
                {
                    Console.WriteLine("Le titulaire du compte ne fait pas parti des clients de la banque !");
                    return;
                }
            }
            else if(typeCompte == 1)
            {
                Console.WriteLine("solde du compte : ");
                double resultDouble;
                double SaisieNombreDouble()
                {
                    while (!double.TryParse(Console.ReadLine(), out resultDouble)) ;
                    return resultDouble;
                }
                double soldeCompte = SaisieNombreDouble();
                Console.WriteLine("découvert max du compte : ");
                double decouvertMaximalAutorise = SaisieNombreDouble();
                Console.WriteLine("débit maximal du compte : ");
                double debitMaximalAutorise = SaisieNombreDouble();
                Console.WriteLine("nom du titulaire : ");
                string nomTitulaire = Console.ReadLine();
                bool verifTitulaire = false;
                foreach (Personne client in clients)
                {
                    if (client.Nom == nomTitulaire)
                    {
                        Personne p = client;
                        verifTitulaire = true;
                        Courant c = new Courant(p, soldeCompte, decouvertMaximalAutorise, debitMaximalAutorise);
                        comptes.Add(c);
                    }
                }
                if (!verifTitulaire)
                {
                    Console.WriteLine("Le titulaire du compte ne fait pas parti des clients de la banque !");
                    return;
                }
            }
        }

        public void SupprimerCompte()
        {
            int result;
            int SaisieNombre()
            {
                while (!int.TryParse(Console.ReadLine(), out result)) ;
                return result;
            }
            Console.WriteLine("Saisir le numéro de compte à supprimer : ");
            int numero = SaisieNombre();
            foreach(Compte compte in comptes)
            {
                if(compte.Numero == numero)
                {
                    comptes.Remove(compte);
                    return;
                }
            }
            Console.WriteLine("Le numéro de compte renseigné n'est pas attribué à un compte existant !");
        }

        public void Virement()
        {
            int result;

            int SaisieNombre()
            {
                while (!int.TryParse(Console.ReadLine(), out result)) ;
                return result;
            }
            Console.WriteLine("Saisir le numéro de compte à crediter :");
            int numeroCompteCrediter = SaisieNombre();
            Console.WriteLine("Saisir le numero de compte à débiter :");
            int numeroCompteDebiter = SaisieNombre();
            Compte c1 = null;
            Compte c2 = null;
            bool verifC1 = false;
            bool verifC2 = false;
            foreach (Compte compte in comptes)
            {
                if(compte.Numero == numeroCompteCrediter)
                {
                    c1 = compte;
                    verifC1 = true;
                }
                else if(compte.Numero == numeroCompteDebiter)
                {
                    c2 = compte;
                    verifC1 = true;
                }
            }
            if(!verifC1)
            {
                Console.WriteLine("Le compte crédité n'existe dans la banque !");
                return;
            }
            else if (!verifC2)
            {
                Console.WriteLine("Le compte débité n'existe dans la banque !");
                return;
            }
            else if(!verifC1 && !verifC2)
            {
                Console.WriteLine("Les comptes crédité et débité n'existe dans la banque !");
                return;
            }
            double resultDouble;
            double SaisieNombreDouble()
            {
                while (!double.TryParse(Console.ReadLine(), out resultDouble)) ;
                return resultDouble;
            }

            double virement = SaisieNombreDouble();
            c1.Crediter(virement);
            c2.Debiter(virement);
            OperationBancaire o1 = new OperationBancaire(virement, "virement depuis le compte numéro " + c2.Numero);
            c1.OperationBancaires.Add(o1);
            OperationBancaire o2 = new OperationBancaire(virement, "virement sur le compte numéro " + c1.Numero);
            c2.OperationBancaires.Add(o2);
        }



    }
    class TestProjet
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;

            int result;

            int SaisieNombre()
            {
                while (!int.TryParse(Console.ReadLine(), out result)) ;
                return result;
            }
            double resultDouble;
            double SaisieNombreDouble()
            {
                while (!double.TryParse(Console.ReadLine(), out resultDouble)) ;
                return resultDouble;
            }

            Console.WriteLine("Saisir le nom de la banque:");
            string nomBanque = Console.ReadLine();
            Banque banque = new Banque(nomBanque);
            do
            {
                Console.Clear();
                Console.WriteLine("Bienvenue dans la banque " + banque.Nom);
                Console.WriteLine("Vous êtes un client (0) ou un banquier (1) ?");
                int typePersonne = SaisieNombre();
                switch (typePersonne)
                {
                    case 0:
                        Console.WriteLine("Veuillez saisir votre nom :");
                        string nomClient = Console.ReadLine();
                        if(banque.Clients == null)
                        {
                            Console.WriteLine("La banque ne possède pas encore de clients! Veuillez contacter un banquier pour vous créer un compte client.");
                            break;
                        }
                        bool verifClient = false;
                        foreach (Personne client in banque.Clients)
                        {
                            if (nomClient.ToLower() == client.Nom.ToLower())
                            {
                                verifClient = true;
                                do
                                {
                                    Console.WriteLine("Bienvenue " + client.Nom);
                                    Console.WriteLine("Menu client`:\n" +
                                                      "action 1: voir les infos de mes comptes" +
                                                      "action 2: créditer un de mes comptes" +
                                                      "action 3: débiter un de mes comptes");
                                    int action = SaisieNombre();
                                    switch (action)
                                    {
                                        case 1:
                                            bool verifCompte = false;
                                            if(banque.Comptes == null)
                                            {
                                                Console.WriteLine("La banque ne possède pas encore de compte actif ! Veuillez contacter un banquier pour créer un compte.");
                                                break;
                                            }
                                            foreach (Compte compte in banque.Comptes)
                                            {
                                                if (compte.TitulaireCompte.Nom.ToLower() == client.Nom.ToLower())
                                                {
                                                    Console.WriteLine(compte.ToString());
                                                    verifCompte = true;
                                                }
                                            }
                                            if (!verifCompte)
                                            {
                                                Console.WriteLine("Vous ne possédez aucun  compte. Prenez contact avec votre banquier pour créer un compte");
                                            }
                                            break;
                                        case 2:
                                            Console.WriteLine("Saisir le numéro du compte à créditer : ");
                                            int numero = SaisieNombre();
                                            bool verifNum = false;
                                            if (banque.Comptes == null)
                                            {
                                                Console.WriteLine("La banque ne possède pas encore de compte actif ! Veuillez contacter un banquier pour créer un compte.");
                                                break;
                                            }
                                            foreach (Compte compte in banque.Comptes)
                                            {
                                                if(compte.Numero == numero)
                                                {
                                                    verifNum = true;
                                                    if(compte.TitulaireCompte.Nom.ToLower() == client.Nom.ToLower())
                                                    {
                                                        Console.WriteLine("Saisir le montant à créditer : ");
                                                        double credit = SaisieNombreDouble();
                                                        compte.Crediter(credit);
                                                        Console.WriteLine("Le compte a été crédité.");
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Le compte que vous souhaitez créditer ne vous appartient pas !");
                                                        break;
                                                    }
                                                }
                                            }
                                            if (!verifNum)
                                            {
                                                Console.WriteLine("Le numéro saisi de correspond pas un numéro de compte dans la banque");
                                            }
                                            break;
                                        case 3:
                                            Console.WriteLine("Saisir le numéro du compte à débiter : ");
                                            numero = SaisieNombre();
                                            verifNum = false;
                                            if (banque.Comptes == null)
                                            {
                                                Console.WriteLine("La banque ne possède pas encore de compte actif ! Veuillez contacter un banquier pour créer un compte.");
                                                break;
                                            }
                                            foreach (Compte compte in banque.Comptes)
                                            {
                                                if (compte.Numero == numero)
                                                {
                                                    verifNum = true;
                                                    if (compte.TitulaireCompte.Nom == client.Nom)
                                                    {
                                                        Console.WriteLine("Saisir le montant à débiter : ");
                                                        double credit = SaisieNombreDouble();
                                                        compte.Crediter(credit);
                                                        Console.WriteLine("Le compte a été débité.");
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Le compte que vous souhaitez débiter ne vous appartient pas !");
                                                        break;
                                                    }
                                                }
                                            }
                                            if (!verifNum)
                                            {
                                                Console.WriteLine("Le numéro saisi de correspond pas un numéro de compte dans la banque !");
                                            }
                                            break;
                                    }
                                    Console.WriteLine("Tapez Escape pour sortir du menu client");
                                    cki = Console.ReadKey();
                                } while (cki.Key != ConsoleKey.Spacebar) ;
                            }
                        }
                        if (!verifClient)
                        {
                            Console.WriteLine("Le client n'existe pas ! Veuillez contacter un banquier pour créer votre compte client.");
                        }
                        break;
                    case 1:
                        break;
                }
                Console.WriteLine("Tapez Escape pour sortir de la banque");
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Spacebar);
        }
    }
}
