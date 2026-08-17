import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { EmployeService } from '../../Services/employe.service';
import { Employe } from '../../models/employe.model';

@Component({
  selector: 'app-ressources-humaines',
  imports: [ReactiveFormsModule, DatePipe],
  templateUrl: './ressources-humaines.html',
  styleUrl: './ressources-humaines.css',
})
export class RessourcesHumaines implements OnInit {
  private employeService = inject(EmployeService);
  private fb = inject(FormBuilder);

  employes = signal<Employe[]>([]);
  erreur = signal<string | null>(null);
  afficherFormulaire = signal(false);
  modeEdition = signal<Employe | null>(null);

  employeForm = this.fb.group({
    nom: ['', Validators.required],
    prenom: ['', Validators.required],
    poste: ['Chauffeur', Validators.required],
    dateEmbauche: ['', Validators.required],
    email: [''],
    telephone: [''],
    statut: ['Actif', Validators.required],
    disponibilite: ['Disponible', Validators.required],
    numeroPermis: [''],
    categoriesPermis: [''],
    expirationPermis: [''],
  });

  ngOnInit(): void {
    this.chargerEmploye();
  }

  chargerEmploye(): void {
    this.employeService.getAll().subscribe({
      next: (data) => this.employes.set(data),
      error: () => this.erreur.set('Erreur de chargement des employés.'),
    });
  }

  ouvrirFormulaire(): void {
    this.modeEdition.set(null);
    this.employeForm.reset({ poste: 'Chauffeur', statut: 'Actif', disponibilite: 'Disponible' });
    this.afficherFormulaire.set(true);
  }

  ouvrirEdition(employe: Employe): void {
    this.modeEdition.set(employe);
    this.employeForm.patchValue({
      nom: employe.nom,
      prenom: employe.prenom,
      poste: employe.poste,
      dateEmbauche: employe.dateEmbauche.slice(0, 10),
      email: employe.email,
      telephone: employe.telephone,
      statut: employe.statut,
      disponibilite: employe.disponibilite,
      numeroPermis: employe.numeroPermis,
      categoriesPermis: employe.categoriesPermis,
      expirationPermis: employe.expirationPermis ? employe.expirationPermis.slice(0, 10) : '',
    });
    this.afficherFormulaire.set(true);
  }

  fermerFormulaire(): void {
    this.afficherFormulaire.set(false);
    this.modeEdition.set(null);
  }

  soumettre(): void {
    if (this.employeForm.invalid) return;

    const valeurs = this.employeForm.value;
    const dto = {
      nom: valeurs.nom!,
      prenom: valeurs.prenom!,
      poste: valeurs.poste!,
      dateEmbauche: valeurs.dateEmbauche!,
      email: valeurs.email ?? '',
      telephone: valeurs.telephone ?? '',
      statut: valeurs.statut!,
      disponibilite: valeurs.disponibilite!,
      numeroPermis: valeurs.numeroPermis ?? '',
      categoriesPermis: valeurs.categoriesPermis ?? '',
      expirationPermis: valeurs.expirationPermis || null,
    };
    const edition = this.modeEdition();

    if (edition) {
      this.employeService.update(edition.idEmploye, dto).subscribe({
        next: () => {
          this.chargerEmploye();
          this.fermerFormulaire();
        },
        error: () => this.erreur.set('Erreur lors de la modification.'),
      });
    } else {
      this.employeService.create(dto).subscribe({
        next: (nouvelEmploye) => {
          this.employes.update((liste) => [...liste, nouvelEmploye]);
          this.fermerFormulaire();
        },
        error: () => this.erreur.set('Erreur lors de la création.'),
      });
    }
  }

  supprimer(id: number): void {
    if (!confirm('Voulez-vous vraiment supprimer cet employé ?')) return;
    this.erreur.set(null);
    this.employeService.delete(id).subscribe({
      next: () => this.employes.update((liste) => liste.filter((e) => e.idEmploye !== id)),
      error: (err) => this.erreur.set(err.error?.message ?? 'Erreur lors de la suppression.'),
    });
  }
}
