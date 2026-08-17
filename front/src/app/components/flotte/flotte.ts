import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { VehiculeService } from '../../Services/vehicule.service';
import { Vehicule } from '../../models/vehicule.model';

@Component({
  selector: 'app-flotte',
  imports: [ReactiveFormsModule, DatePipe],
  templateUrl: './flotte.html',
  styleUrl: './flotte.css',
})
export class Flotte implements OnInit {
  private vehiculeService = inject(VehiculeService);
  private fb = inject(FormBuilder);

  vehicules = signal<Vehicule[]>([]);
  erreur = signal<string | null>(null);
  afficherFormulaire = signal(false);
  modeEdition = signal<Vehicule | null>(null);

  vehiculeForm = this.fb.group({
    immatriculation: ['', Validators.required],
    typeVehicule: ['', Validators.required],
    capaciteKg: [0, [Validators.required, Validators.min(0)]],
    kilometrage: [0, [Validators.required, Validators.min(0)]],
    statut: ['Disponible', Validators.required],
    dernierEntretien: [''],
    prochainEntretien: [''],
    prochainControleTechnique: [''],
    finAssurance: [''],
  });

  ngOnInit(): void {
    this.chargerVehicule();
  }

  chargerVehicule(): void {
    this.vehiculeService.getAll().subscribe({
      next: (data) => this.vehicules.set(data),
      error: () => this.erreur.set('Erreur de chargement de la flotte.'),
    });
  }

  ouvrirFormulaire(): void {
    this.modeEdition.set(null);
    this.vehiculeForm.reset({ statut: 'Disponible', capaciteKg: 0, kilometrage: 0 });
    this.afficherFormulaire.set(true);
  }

  ouvrirEdition(vehicule: Vehicule): void {
    this.modeEdition.set(vehicule);
    this.vehiculeForm.patchValue({
      immatriculation: vehicule.immatriculation,
      typeVehicule: vehicule.typeVehicule,
      capaciteKg: vehicule.capaciteKg,
      kilometrage: vehicule.kilometrage,
      statut: vehicule.statut,
      dernierEntretien: this.versDateInput(vehicule.dernierEntretien),
      prochainEntretien: this.versDateInput(vehicule.prochainEntretien),
      prochainControleTechnique: this.versDateInput(vehicule.prochainControleTechnique),
      finAssurance: this.versDateInput(vehicule.finAssurance),
    });
    this.afficherFormulaire.set(true);
  }

  fermerFormulaire(): void {
    this.afficherFormulaire.set(false);
    this.modeEdition.set(null);
  }

  soumettre(): void {
    if (this.vehiculeForm.invalid) return;

    const valeurs = this.vehiculeForm.value;
    const dto = {
      immatriculation: valeurs.immatriculation!,
      typeVehicule: valeurs.typeVehicule!,
      capaciteKg: Number(valeurs.capaciteKg),
      kilometrage: Number(valeurs.kilometrage),
      statut: valeurs.statut!,
      dernierEntretien: valeurs.dernierEntretien || null,
      prochainEntretien: valeurs.prochainEntretien || null,
      prochainControleTechnique: valeurs.prochainControleTechnique || null,
      finAssurance: valeurs.finAssurance || null,
    };
    const edition = this.modeEdition();

    if (edition) {
      this.vehiculeService.update(edition.idVehicule, dto).subscribe({
        next: () => {
          this.chargerVehicule();
          this.fermerFormulaire();
        },
        error: () => this.erreur.set('Erreur lors de la modification.'),
      });
    } else {
      this.vehiculeService.create(dto).subscribe({
        next: (nouveauVehicule) => {
          this.vehicules.update((liste) => [...liste, nouveauVehicule]);
          this.fermerFormulaire();
        },
        error: () => this.erreur.set('Erreur lors de la création.'),
      });
    }
  }

  supprimer(id: number): void {
    if (!confirm('Voulez-vous vraiment supprimer ce véhicule ?')) return;
    this.erreur.set(null);
    this.vehiculeService.delete(id).subscribe({
      next: () => this.vehicules.update((liste) => liste.filter((v) => v.idVehicule !== id)),
      error: (err) => this.erreur.set(err.error?.message ?? 'Erreur lors de la suppression.'),
    });
  }

  private versDateInput(valeur: string | null): string {
    return valeur ? valeur.slice(0, 10) : '';
  }
}
