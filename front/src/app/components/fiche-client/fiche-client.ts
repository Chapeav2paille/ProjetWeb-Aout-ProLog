import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ClientService } from '../../Services/client.service';
import { Client } from '../../models/client.model';
import { Prestation } from '../../models/prestation.model';

@Component({
  selector: 'app-fiche-client',
  imports: [DatePipe, RouterLink],
  templateUrl: './fiche-client.html',
  styleUrl: './fiche-client.css',
})
export class FicheClient implements OnInit {
  private clientService = inject(ClientService);
  private route = inject(ActivatedRoute);

  client = signal<Client | null>(null);
  prestations = signal<Prestation[]>([]);
  erreur = signal<string | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.clientService.getById(id).subscribe({
      next: (data) => this.client.set(data),
      error: () => this.erreur.set('Client introuvable.'),
    });

    this.clientService.getPrestations(id).subscribe({
      next: (data) => this.prestations.set(data),
      error: () => this.erreur.set('Erreur de chargement des prestations.'),
    });
  }
}
