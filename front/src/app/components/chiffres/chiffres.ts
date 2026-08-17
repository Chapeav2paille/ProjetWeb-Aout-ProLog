import { Component, inject, OnInit, signal } from '@angular/core';
import { DecimalPipe, PercentPipe } from '@angular/common';
import { ChiffresService } from '../../Services/chiffres.service';
import { Chiffres as ChiffresModel } from '../../models/chiffres.model';

@Component({
  selector: 'app-chiffres',
  imports: [DecimalPipe, PercentPipe],
  templateUrl: './chiffres.html',
  styleUrl: './chiffres.css',
})
export class Chiffres implements OnInit {
  private chiffresService = inject(ChiffresService);

  chiffres = signal<ChiffresModel | null>(null);
  erreur = signal<string | null>(null);

  ngOnInit(): void {
    this.chargerChiffres();
  }

  chargerChiffres(): void {
    this.chiffresService.get().subscribe({
      next: (data) => this.chiffres.set(data),
      error: () => this.erreur.set('Erreur de chargement des chiffres.'),
    });
  }
}
