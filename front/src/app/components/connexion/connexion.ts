import { Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-connexion',
  imports: [ReactiveFormsModule],
  templateUrl: './connexion.html',
  styleUrl: './connexion.css',
})
export class Connexion {
  private authService = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  erreur = signal<string | null>(null);
  enCours = signal(false);

  connexionForm = this.fb.group({
    nomUtilisateur: ['', Validators.required],
    motDePasse: ['', Validators.required],
  });

  soumettre(): void {
    if (this.connexionForm.invalid) return;

    this.erreur.set(null);
    this.enCours.set(true);

    const valeurs = this.connexionForm.value;
    this.authService.connexion({
      nomUtilisateur: valeurs.nomUtilisateur!,
      motDePasse: valeurs.motDePasse!,
    }).subscribe({
      next: () => this.router.navigate(['/tableau-bord']),
      error: (err) => {
        this.enCours.set(false);
        this.erreur.set(err.status === 401
          ? 'Identifiants invalides.'
          : 'Impossible de contacter le serveur.');
      },
    });
  }
}
