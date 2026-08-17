import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { TableauBordService } from '../../Services/tableau-bord.service';
import { TableauBord } from '../../models/tableau-bord.model';

interface BarreGraphique {
  periode: string;
  montant: number;
  x: number;
  y: number;
  hauteur: number;
}

@Component({
  selector: 'app-tableau-bord',
  imports: [DatePipe, DecimalPipe],
  templateUrl: './tableau-bord.html',
  styleUrl: './tableau-bord.css',
})
export class TableauBordComponent implements OnInit {
  private tableauBordService = inject(TableauBordService);

  readonly largeurGraphique = 660;
  readonly hauteurGraphique = 200;

  tableauBord = signal<TableauBord | null>(null);
  erreur = signal<string | null>(null);

  montantMaximum = computed(() => {
    const points = this.tableauBord()?.evolutionChiffreAffaire ?? [];
    return Math.max(1, ...points.map((point) => point.montant));
  });

  barres = computed<BarreGraphique[]>(() => {
    const points = this.tableauBord()?.evolutionChiffreAffaire ?? [];
    if (points.length === 0) return [];

    const maximum = this.montantMaximum();
    const largeurColonne = this.largeurGraphique / points.length;

    return points.map((point, index) => {
      const hauteur = (point.montant / maximum) * this.hauteurGraphique;
      return {
        periode: point.periode,
        montant: point.montant,
        x: index * largeurColonne + largeurColonne * 0.2,
        y: this.hauteurGraphique - hauteur,
        hauteur,
      };
    });
  });

  largeurBarre = computed(() => {
    const nombre = this.tableauBord()?.evolutionChiffreAffaire.length ?? 1;
    return (this.largeurGraphique / nombre) * 0.6;
  });

  ngOnInit(): void {
    this.chargerTableauBord();
  }

  chargerTableauBord(): void {
    this.tableauBordService.get().subscribe({
      next: (data) => this.tableauBord.set(data),
      error: () => this.erreur.set('Erreur de chargement du tableau de bord.'),
    });
  }
}
