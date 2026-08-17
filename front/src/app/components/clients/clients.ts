import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ClientService } from '../../Services/client.service';
import { Client } from '../../models/client.model';

@Component({
  selector: 'app-clients',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './clients.html',
  styleUrl: './clients.css',
})
export class Clients implements OnInit {
  private clientService = inject(ClientService);
  private fb = inject(FormBuilder);

  clients = signal<Client[]>([]);
  erreur = signal<string | null>(null);
  afficherFormulaire = signal(false);
  modeEdition = signal<Client | null>(null);

  clientForm = this.fb.group({
    typeClient: ['Entreprise', Validators.required],
    nom: ['', Validators.required],
    contact: [''],
    email: [''],
    telephone: [''],
    adresse: [''],
  });

  ngOnInit(): void {
    this.chargerClient();
  }

  chargerClient(): void {
    this.clientService.getAll().subscribe({
      next: (data) => this.clients.set(data),
      error: () => this.erreur.set('Erreur de chargement des clients.'),
    });
  }

  ouvrirFormulaire(): void {
    this.modeEdition.set(null);
    this.clientForm.reset({ typeClient: 'Entreprise' });
    this.afficherFormulaire.set(true);
  }

  ouvrirEdition(client: Client): void {
    this.modeEdition.set(client);
    this.clientForm.patchValue({
      typeClient: client.typeClient,
      nom: client.nom,
      contact: client.contact,
      email: client.email,
      telephone: client.telephone,
      adresse: client.adresse,
    });
    this.afficherFormulaire.set(true);
  }

  fermerFormulaire(): void {
    this.afficherFormulaire.set(false);
    this.modeEdition.set(null);
  }

  soumettre(): void {
    if (this.clientForm.invalid) return;

    const valeurs = this.clientForm.value;
    const dto = {
      typeClient: valeurs.typeClient!,
      nom: valeurs.nom!,
      contact: valeurs.contact ?? '',
      email: valeurs.email ?? '',
      telephone: valeurs.telephone ?? '',
      adresse: valeurs.adresse ?? '',
    };
    const edition = this.modeEdition();

    if (edition) {
      this.clientService.update(edition.idClient, dto).subscribe({
        next: () => {
          this.chargerClient();
          this.fermerFormulaire();
        },
        error: () => this.erreur.set('Erreur lors de la modification.'),
      });
    } else {
      this.clientService.create(dto).subscribe({
        next: (nouveauClient) => {
          this.clients.update((liste) => [...liste, nouveauClient]);
          this.fermerFormulaire();
        },
        error: () => this.erreur.set('Erreur lors de la création.'),
      });
    }
  }

  supprimer(id: number): void {
    if (!confirm('Voulez-vous vraiment supprimer ce client ?')) return;
    this.erreur.set(null);
    this.clientService.delete(id).subscribe({
      next: () => this.clients.update((liste) => liste.filter((c) => c.idClient !== id)),
      error: (err) => this.erreur.set(err.error?.message ?? 'Erreur lors de la suppression.'),
    });
  }
}
