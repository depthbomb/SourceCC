using System.Collections.Generic;

namespace SourceCC.Classes
{
    class I18n
    {
        private static readonly string Language = Properties.Settings.Default.Language;

        //
        //  A basic internationalization system.
        //  Some strings in the code will not be translated as they are impossible to be seen
        //  when another language is active.
        //
        public static string __(string key, params string[] replacements)
        {
            string lang = Language;
            int replacementCount = replacements.Length;
            IDictionary<string, string> dict = new Dictionary<string, string>();
            switch (lang)
            {
                #region English
                default:
                case "en":
                    //  Cleaning process
                    dict["process_started"] = "Processing...";
                    dict["process_deleted_file"] = "Deleted {0}";
                    dict["process_already_clean"] = "Wow! This game folder is already clean!";
                    dict["process_total_deleted"] = "Deleted {0} files(s)";
                    dict["process_completed"] = "Finished process in {0}ms.";

                    //  Non-existent path error
                    dict["folder_not_found_text"] = "{0} could not be located. If your {1} installation is in a different location then please change it in settings.\n\nWould you like to open settings now?";
                    dict["folder_not_found_caption"] = "Folder not found";

                    //  Button text
                    dict["settings"] = "Settings";
                    dict["change_folder"] = "Change";
                    dict["begin_process"] = "Clean it!";

                    //  Label text
                    dict["language_label"] = "Language";
                    break;
                #endregion
                #region Spanish
                case "es":
                    dict["process_started"] = "Trabajando...";
                    dict["process_deleted_file"] = "Eliminado {0}";
                    dict["process_already_clean"] = "¡Caray! ¡La carpeta del juego ya se ha limpiado!";
                    dict["process_total_deleted"] = "{0} archivos eliminados";
                    dict["process_completed"] = "Proceso completado en {0}ms";

                    dict["folder_not_found_text"] = "{0} no se pudo encontrar. Si su carpeta {1} está en una ubicación diferente, cámbiela en su configuración.\n\n¿Desea abrir la configuración ahora ? ";
                    dict["folder_not_found_caption"] = "Carpeta no encontrada";

                    dict["settings"] = "Configuraciones";
                    dict["change_folder"] = "Cambiar";
                    dict["begin_process"] = "¡Limpialo!";

                    dict["language_label"] = "Idioma";
                    break;
                #endregion
                
            }

            if (replacementCount < 1)
            {
                return dict[key];
            }
            else
            {
                string i18nString = dict[key];
                for (int i = 0; i < replacementCount; i++)
                {
                    string token = $"{{{i}}}";
                    string rep   = replacements[i];

                    i18nString = i18nString.Replace(token, rep);
                }

                return i18nString;
            }
        }
    }
}
