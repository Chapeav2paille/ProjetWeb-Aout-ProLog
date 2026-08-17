import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../Services/auth.service';

export const authInterceptor: HttpInterceptorFn = (requete, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const token = authService.getToken();

  const requeteAuthentifiee = token
    ? requete.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : requete;

  return next(requeteAuthentifiee).pipe(
    catchError((erreur: HttpErrorResponse) => {
      if (erreur.status === 401 && !requete.url.includes('/Auth/connexion')) {
        authService.deconnexion();
        router.navigate(['/connexion']);
      }
      return throwError(() => erreur);
    })
  );
};
