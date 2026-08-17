import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { HistoriqueService } from '../../Services/historique.service';
import { Historique } from '../../models/historique.model';

@Component({
  selector: 'app-historique',
  imports: [DatePipe],
  templateUrl: './historique.html',
  styleUrl: './historique.css',
})
export class HistoriqueComponent implements OnInit {
  private historiqueService = inject(HistoriqueService);

  historique = signal<Historique[]>([]);
  erreur = signal<string | null>(null);
  du = signal('');
  au = signal('');
  typeAction = signal('');

  ngOnInit(): void {
    this.chargerHistorique();
  }

  chargerHistorique(): void {
    this.historiqueService
      .getAll(this.du() || undefined, this.au() || undefined, this.typeAction() || undefined)
      .subscribe({
        next: (data) => this.historique.set(data),
        error: () => this.erreur.set("Erreur de chargement de l'historique."),
      });
  }

  changerDu(valeur: string): void {
    this.du.set(valeur);
    this.chargerHistorique();
  }

  changerAu(valeur: string): void {
    this.au.set(valeur);
    this.chargerHistorique();
  }

  changerTypeAction(valeur: string): void {
    this.typeAction.set(valeur);
    this.chargerHistorique();
  }
}
