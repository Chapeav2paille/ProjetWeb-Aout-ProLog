import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { PrestationService } from '../../Services/prestation.service';
import { ClientService } from '../../Services/client.service';
import { VehiculeService } from '../../Services/vehicule.service';
import { EmployeService } from '../../Services/employe.service';
import { Prestation } from '../../models/prestation.model';
import { Client } from '../../models/client.model';
import { Vehicule } from '../../models/vehicule.model';
import { Employe } from '../../models/employe.model';

@Component({
  selector: 'app-prestations',
  imports: [ReactiveFormsModule, DatePipe],
  templateUrl: './prestations.html',
  styleUrl: './prestations.css',
})
export class Prestations implements OnInit {
  private prestationService = inject(PrestationService);
  private clientService = inject(ClientService);
  private vehiculeService = inject(VehiculeService);
  private employeService = inject(EmployeService);
  private fb = inject(FormBuilder);

  prestations = signal<Prestation[]>([]);
  clients = signal<Client[]>([]);
  vehicules = signal<Vehicule[]>([]);
  chauffeurs = signal<Employe[]>([]);
  erreur = signal<string | null>(null);
  afficherFormulaire = signal(false);
  modeEdition = signal<Prestation | null>(null);
  filtreStatut = signal('');

  prestationForm = this.fb.group({
    idClient: [null as number | null, Validators.required],
    idVehicule: [null as number | null],
    idEmploye: [null as number | null],
    adresseDepart: ['', Validators.required],
    adresseArrivee: ['', Validators.required],
    dateHeure: ['', Validators.required],
    typeService: ['', Validators.required],
    prix: [0, [Validators.required, Validators.min(0)]],
    statut: ['Planifiee', Validators.required],
  });

  ngOnInit(): void {
    this.chargerPrestation();
    this.clientService.getAll().subscribe((data) => this.clients.set(data));
    this.vehiculeService.getAll().subscribe((data) => this.vehicules.set(data));
    this.employeService.getAll().subscribe((data) =>
      this.chauffeurs.set(data.filter((e) => e.poste === 'Chauffeur' && e.statut === 'Actif')));
  }

  chargerPrestation(): void {
    this.prestationService.getAll(this.filtreStatut() || undefined).subscribe({
      next: (data) => this.prestations.set(data),
      error: () => this.erreur.set('Erreur de chargement des prestations.'),
    });
  }

  changerFiltre(statut: string): void {
    this.filtreStatut.set(statut);
    this.chargerPrestation();
  }

  ouvrirFormulaire(): void {
    this.modeEdition.set(null);
    this.prestationForm.reset({ statut: 'Planifiee', prix: 0, idVehicule: null, idEmploye: null });
    this.afficherFormulaire.set(true);
  }

  ouvrirEdition(prestation: Prestation): void {
    this.modeEdition.set(prestation);
    this.prestationForm.patchValue({
      idClient: prestation.idClient,
      idVehicule: prestation.idVehicule,
      idEmploye: prestation.idEmploye,
      adresseDepart: prestation.adresseDepart,
      adresseArrivee: prestation.adresseArrivee,
      dateHeure: prestation.dateHeure.slice(0, 16),
      typeService: prestation.typeService,
      prix: prestation.prix,
      statut: prestation.statut,
    });
    this.afficherFormulaire.set(true);
  }

  fermerFormulaire(): void {
    this.afficherFormulaire.set(false);
    this.modeEdition.set(null);
  }

  soumettre(): void {
    if (this.prestationForm.invalid) return;

    const valeurs = this.prestationForm.value;
    const dto = {
      idClient: Number(valeurs.idClient),
      idVehicule: valeurs.idVehicule ? Number(valeurs.idVehicule) : null,
      idEmploye: valeurs.idEmploye ? Number(valeurs.idEmploye) : null,
      adresseDepart: valeurs.adresseDepart!,
      adresseArrivee: valeurs.adresseArrivee!,
      dateHeure: valeurs.dateHeure!,
      typeService: valeurs.typeService!,
      prix: Number(valeurs.prix),
      statut: valeurs.statut!,
    };
    const edition = this.modeEdition();

    if (edition) {
      this.prestationService.update(edition.idPrestation, dto).subscribe({
        next: () => {
          this.chargerPrestation();
          this.fermerFormulaire();
        },
        error: () => this.erreur.set('Erreur lors de la modification.'),
      });
    } else {
      this.prestationService.create(dto).subscribe({
        next: () => {
          this.chargerPrestation();
          this.fermerFormulaire();
        },
        error: () => this.erreur.set('Erreur lors de la création.'),
      });
    }
  }

  changerStatut(prestation: Prestation, statut: string): void {
    this.prestationService.changerStatut(prestation.idPrestation, statut).subscribe({
      next: () => this.chargerPrestation(),
      error: () => this.erreur.set('Erreur lors du changement de statut.'),
    });
  }

  supprimer(id: number): void {
    if (!confirm('Voulez-vous vraiment supprimer cette prestation ?')) return;
    this.prestationService.delete(id).subscribe({
      next: () => this.prestations.update((liste) => liste.filter((p) => p.idPrestation !== id)),
      error: () => this.erreur.set('Erreur lors de la suppression.'),
    });
  }
}
