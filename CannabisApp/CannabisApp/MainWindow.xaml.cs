using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CannabisApp
{
    public partial class MainWindow : Window
    {
        internal class PlantRepository
        {
            private readonly AppDbContext _context;

            public PlantRepository(AppDbContext context)
            {
                _context = context;
            }

            public void AddPlante(Plantes plante)
            {
                _context.Plantes.Add(plante);
                _context.SaveChanges();
            }

            public void UpdatePlante(Plantes plante)
            {
                var existingPlante = _context.Plantes.Find(plante.IdPlante);
                if (existingPlante != null)
                {
                  
                   
                    existingPlante.CodeQr = plante.CodeQr;
                    existingPlante.IdProvenance = plante.IdProvenance;
                    existingPlante.EtatSante = plante.EtatSante;
                    existingPlante.NombrePlantesActives = plante.NombrePlantesActives;
                    existingPlante.DateExpiration = plante.DateExpiration;
                    existingPlante.CreeLe = plante.CreeLe;

                    _context.SaveChanges();
                }
            }

            public void DeletePlante(int idPlante)
            {
                var plante = _context.Plantes.Find(idPlante);
                if (plante != null)
                {
                    _context.Plantes.Remove(plante);
                    _context.SaveChanges();
                }
            }

            public List<Plantes> GetPlantes()
            {
                return _context.Plantes.ToList();
            }

            public Plantes GetPlanteById(int idPlante)
            {
                return _context.Plantes.Find(idPlante);
            }
        }

        private readonly PlantRepository _plantRepository;

        internal class UserRepository
        {
            private readonly AppDbContext _context;

            public UserRepository(AppDbContext context)
            {
                _context = context;
            }

            public void AddUser(Utilisateur user)
            {
                _context.Utilisateurs.Add(user);
                _context.SaveChanges();
            }

            public void UpdateUser(Utilisateur user)
            {
                var existingUser = _context.Utilisateurs.Find(user.IdUtilisateur);
                if (existingUser != null)
                {
                    existingUser.NomUtilisateur = user.NomUtilisateur;
                    existingUser.MotDePasse = user.MotDePasse;
                    existingUser.IdRole = user.IdRole;

                    _context.SaveChanges();
                }
            }

            public void DeleteUser(int userId)
            {
                var user = _context.Utilisateurs.Find(userId);
                if (user != null)
                {
                    _context.Utilisateurs.Remove(user);
                    _context.SaveChanges();
                }
            }

            public List<Utilisateur> GetUsers()
            {
                return _context.Utilisateurs.ToList();
            }

            public Utilisateur GetUserById(int userId)
            {
                return _context.Utilisateurs.Find(userId);
            }
        }

        private readonly UserRepository _userRepository;

        internal class RoleRepository
        {
            private readonly AppDbContext _context;

            public RoleRepository(AppDbContext context)
            {
                _context = context;
            }

            public void AddRole(Roles role)
            {
                _context.Roles.Add(role);
                _context.SaveChanges();
            }

            public void UpdateRole(Roles role)
            {
                var existingRole = _context.Roles.Find(role.IdRole);
                if (existingRole != null)
                {
                    existingRole.NomRole = role.NomRole;

                    _context.SaveChanges();
                }
            }

            public void DeleteRole(int roleId)
            {
                var role = _context.Roles.Find(roleId);
                if (role != null)
                {
                    _context.Roles.Remove(role);
                    _context.SaveChanges();
                }
            }

            public List<Roles> GetRoles()
            {
                return _context.Roles.ToList();
            }

            public Roles GetRoleById(int roleId)
            {
                return _context.Roles.Find(roleId);
            }
        }

        private readonly RoleRepository _roleRepository;

        internal class ProvenanceRepository
        {
            private readonly AppDbContext _context;

            public ProvenanceRepository(AppDbContext context)
            {
                _context = context;
            }

            public void AddProvenance(Provenances provenance)
            {
                _context.Provenances.Add(provenance);
                _context.SaveChanges();
            }

            public void UpdateProvenance(Provenances provenance)
            {
                var existingProvenance = _context.Provenances.Find(provenance.IdProvenance);
                if (existingProvenance != null)
                {
                    existingProvenance.Ville = provenance.Ville;
                    existingProvenance.Province = provenance.Province;
                    existingProvenance.Pays = provenance.Pays;

                    _context.SaveChanges();
                }
            }

            public void DeleteProvenance(int provenanceId)
            {
                var provenance = _context.Provenances.Find(provenanceId);
                if (provenance != null)
                {
                    _context.Provenances.Remove(provenance);
                    _context.SaveChanges();
                }
            }

            public List<Provenances> GetProvenances()
            {
                return _context.Provenances.ToList();
            }

            public Provenances GetProvenanceById(int provenanceId)
            {
                return _context.Provenances.Find(provenanceId);
            }
        }

        private readonly ProvenanceRepository _provenanceRepository;

        internal class InventaireRepository
        {
            private readonly AppDbContext _context;

            public InventaireRepository(AppDbContext context)
            {
                _context = context;
            }

            public void AddInventaire(Inventaire inventaire)
            {
                _context.Inventaire.Add(inventaire);
                _context.SaveChanges();
            }

            public void UpdateInventaire(Inventaire inventaire)
            {
                var existingInventaire = _context.Inventaire.Find(inventaire.IdInventaire);
                if (existingInventaire != null)
                {
                    existingInventaire.IdPlante = inventaire.IdPlante;
                    existingInventaire.Quantite = inventaire.Quantite;
                    existingInventaire.DerniereVerification = inventaire.DerniereVerification;

                    _context.SaveChanges();
                }
            }

            public void DeleteInventaire(int inventaireId)
            {
                var inventaire = _context.Inventaire.Find(inventaireId);
                if (inventaire != null)
                {
                    _context.Inventaire.Remove(inventaire);
                    _context.SaveChanges();
                }
            }

            public List<Inventaire> GetInventaires()
            {
                return _context.Inventaire.ToList();
            }

            public Inventaire GetInventaireById(int inventaireId)
            {
                return _context.Inventaire.Find(inventaireId);
            }
        }

        private readonly InventaireRepository _inventaireRepository;

        internal class HistoriquePlantesRepository
        {
            private readonly AppDbContext _context;

            public HistoriquePlantesRepository(AppDbContext context)
            {
                _context = context;
            }

            public void AddHistoriquePlantes(Historique_Plantes historiquePlantes)
            {
                _context.HistoriquePlantes.Add(historiquePlantes);
                _context.SaveChanges();
            }

            public void UpdateHistoriquePlantes(Historique_Plantes historiquePlantes)
            {
                var existingHistoriquePlantes = _context.HistoriquePlantes.Find(historiquePlantes.IdHistorique);
                if (existingHistoriquePlantes != null)
                {
                    existingHistoriquePlantes.IdPlante = historiquePlantes.IdPlante;
                    existingHistoriquePlantes.Action = historiquePlantes.Action;
                    existingHistoriquePlantes.Timestamp = historiquePlantes.Timestamp;
                    existingHistoriquePlantes.IdUtilisateur = historiquePlantes.IdUtilisateur;

                    _context.SaveChanges();
                }
            }

            public void DeleteHistoriquePlantes(int historiquePlantesId)
            {
                var historiquePlantes = _context.HistoriquePlantes.Find(historiquePlantesId);
                if (historiquePlantes != null)
                {
                    _context.HistoriquePlantes.Remove(historiquePlantes);
                    _context.SaveChanges();
                }
            }

            public List<Historique_Plantes> GetHistoriquePlantes()
            {
                return _context.HistoriquePlantes.ToList();
            }

            public Historique_Plantes GetHistoriquePlantesById(int historiquePlantesId)
            {
                return _context.HistoriquePlantes.Find(historiquePlantesId);
            }
        }

        private readonly HistoriquePlantesRepository _historiquePlantesRepository;

        public MainWindow()
        {
            _plantRepository = new PlantRepository(new AppDbContext());
            _userRepository = new UserRepository(new AppDbContext());
            _roleRepository = new RoleRepository(new AppDbContext());
            _provenanceRepository = new ProvenanceRepository(new AppDbContext());
            _inventaireRepository = new InventaireRepository(new AppDbContext());
            _historiquePlantesRepository = new HistoriquePlantesRepository(new AppDbContext());

            InitializeComponent();

            MainFrame.Navigate(new Page1());
        }

        private void NomDutilisateur_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Exemple de méthode pour le TextChanged event
        }

        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            // Exemple de méthode pour le Click event
        }
    }
}
