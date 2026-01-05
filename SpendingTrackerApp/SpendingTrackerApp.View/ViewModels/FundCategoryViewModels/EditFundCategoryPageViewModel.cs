using AutoMapper;
using Microsoft.Extensions.Logging;
using SpendingTrackerApp.Contracts.Dtos.Requests;
using SpendingTrackerApp.Domain.Models;
using SpendingTrackerApp.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Windows.Input;

namespace SpendingTrackerApp.ViewModels.FundCategoryViewModels
{
	[QueryProperty(nameof(Domain.Models.FundCategory), nameof(Domain.Models.FundCategory))]
	public class EditFundCategoryPageViewModel : INotifyPropertyChanged
	{
		private User _user;
		private FundCategoryRequest _fundCategoryRequest = new FundCategoryRequest();
		private IFundCategotuService _fundCategoryService;
		private IMapper _mapper;
		private ILogger<EditFundCategoryPageViewModel> _logger;

		private string _message = "Format: 00.00. Do 15 przed piecinkiem, 2 po przecinku. Jedynie liczby pozytywne. Nazwa Wymagana";
		private Color _messageColor = (Color)Application.Current.Resources["Positive"];
		private Color _shouldBeEntryColor = Colors.White;
		public bool _showLoadingIcon = false;
		public bool _runLoadingIcon = false;

		public bool _blockInteraction = false;

		private FundCategory _fundCategory;
		public FundCategory FundCategory
		{
			get => _fundCategory;
			set
			{
				if (_fundCategory != value)
				{
					_fundCategory = value;
					OnPropertyChanged(nameof(FundCategory));
				}
			}
		}

		public FundCategoryRequest FundCategoryRequest
		{
			get => _fundCategoryRequest;
			set
			{
				if (_fundCategoryRequest != value)
				{
					_fundCategoryRequest = value;
					OnPropertyChanged(nameof(FundCategoryRequest));
				}
			}
		}

		public string Message
		{
			get => _message;
			set
			{
				if (_message != value)
				{
					_message = value;
					OnPropertyChanged(nameof(Message));
				}
			}
		}

		public Color MessageColor
		{
			get => _messageColor;
			set
			{
				if (_messageColor != value)
				{
					_messageColor = value;
					OnPropertyChanged(nameof(MessageColor));
				}
			}
		}

		public Color ShouldBeEntryColor
		{
			get => _shouldBeEntryColor;
			set
			{
				if (_shouldBeEntryColor != value)
				{
					_shouldBeEntryColor = value;
					OnPropertyChanged(nameof(ShouldBeEntryColor));
				}
			}
		}

		public bool ShowLoadingIcon
		{
			get => _showLoadingIcon;
			set
			{
				if (_showLoadingIcon != value)
				{
					_showLoadingIcon = value;
					OnPropertyChanged(nameof(ShowLoadingIcon));
				}
			}
		}

		public bool RunLoadingIcon
		{
			get => _runLoadingIcon;
			set
			{
				if (_runLoadingIcon != value)
				{
					_runLoadingIcon = value;
					OnPropertyChanged(nameof(RunLoadingIcon));
				}
			}
		}

		public bool BlockInteraction
		{
			get => _blockInteraction;
			set
			{
				if (_blockInteraction != value)
				{
					_blockInteraction = value;
					OnPropertyChanged(nameof(BlockInteraction));
				}
			}
		}


		public EditFundCategoryPageViewModel(
		User user,
		IFundCategotuService fundCategoryService,
		IMapper mapper,
		ILogger<EditFundCategoryPageViewModel> logger)
		{
			_user = user;
			_fundCategoryService = fundCategoryService;
			_mapper = mapper;
			_logger = logger;

			EditFundCategoryCommand = new Command(async () => await EditFundCategory());
			CancelEditCommand = new Command(async () => await CancelEdit());
		}
		public ICommand EditFundCategoryCommand { get; }
		public ICommand CancelEditCommand { get; }

		public async Task Reset()
		{
			_logger.LogInformation("Rozpoczynam resetowanie formularza kategorii funduszu oraz stanu błędów UI.");

			FundCategoryRequest = new FundCategoryRequest() { Name = FundCategory.Name };
			Message = "Format: 00.00. Do 15 przed przecinkiem, 2 po przecinku. Jedynie liczby dodatnie. Nazwa wymagana.";
			MessageColor = ShouldBeEntryColor = (Color)Application.Current.Resources["Positive"];

			_logger.LogInformation("Zakończono resetowanie formularza kategorii funduszu oraz stanu błędów UI.");
		}

		public async Task EditFundCategory()
		{
			_logger.LogInformation(
				"Rozpoczynam proces edycji kategorii funduszu. FundCategoryId={FundCategoryId}, OldShouldBe={OldShouldBe}, NewShouldBe={NewShouldBe}",
				FundCategory.Id,
				FundCategory.ShouldBe,
				FundCategoryRequest.ShouldBe
			);

			ShowLoadingIcon = true;
			RunLoadingIcon = true;
			BlockInteraction = true;

			try
			{
				if (String.IsNullOrEmpty(FundCategoryRequest.Name))
				{
					_logger.LogWarning(
						"Brak nazwy kategori. Name={Name}, ShouldBe={ShouldBe}",
						FundCategoryRequest.Name,
						FundCategoryRequest.ShouldBe
					);
					Message = "Nazwa jest wymagana.";
					MessageColor = (Color)Application.Current.Resources["Negative"];
					return;
				}

				bool isShouldBeValid = CheckShouldBe();
				if (!isShouldBeValid)
				{
					_logger.LogWarning(
						"Walidacja wartości ShouldBe dla kategorii funduszu nie powiodła się. FundCategoryId={FundCategoryId}, ShouldBe={ShouldBe}",
						FundCategory.Id,
						FundCategoryRequest.ShouldBe
					);

					MessageColor = ShouldBeEntryColor = (Color)Application.Current.Resources["Negative"];
					Message = "Wartość powinno być jest niepoprawna.";
					return;
				}

				FundCategoryRequest.Id = FundCategory.Id;

				var response = await _fundCategoryService.EditFundCategory(FundCategoryRequest);

				_logger.LogInformation(
					"Wynik edycji kategorii funduszu: FundCategoryId={FundCategoryId}, StatusCode={StatusCode}",
					FundCategory.Id,
					response.StatusCode
				);

				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();

					_logger.LogWarning(
						"Edycja kategorii funduszu nie powiodła się. FundCategoryId={FundCategoryId}, StatusCode={StatusCode}, Content={Content}",
						FundCategory.Id,
						response.StatusCode,
						content
					);

					FundCategory.Name = FundCategoryRequest.Name;
					if (FundCategory.ShouldBe != FundCategoryRequest.ShouldBe)
					{
						FundCategory.ShouldBe = FundCategoryRequest.ShouldBe;
					}

					MessageColor = (Color)Application.Current.Resources["Negative"];
					Message = "Coś poszło nie tak. Spróbuj ponownie.";
					return;
				}

				FundCategory.ShouldBe = FundCategoryRequest.ShouldBe;

				MessageColor = ShouldBeEntryColor = (Color)Application.Current.Resources["Positive"];
				Message = "Kategoria funduszu została zaktualizowana pomyślnie.";

				_logger.LogInformation(
					"Edycja kategorii funduszu zakończona sukcesem. FundCategoryId={FundCategoryId}, NewShouldBe={NewShouldBe}",
					FundCategory.Id,
					FundCategoryRequest.ShouldBe
				);
			}
			catch (HttpRequestException httpEx)
			{
				_logger.LogError(
					httpEx,
					"Błąd HTTP podczas edycji kategorii funduszu. FundCategoryId={FundCategoryId}, ShouldBe={ShouldBe}",
					FundCategory.Id,
					FundCategoryRequest.ShouldBe
				);

				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Błąd sieci. Spróbuj ponownie.";
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas edycji kategorii funduszu. FundCategoryId={FundCategoryId}, ShouldBe={ShouldBe}",
					FundCategory.Id,
					FundCategoryRequest.ShouldBe
				);

				MessageColor = (Color)Application.Current.Resources["Negative"];
				Message = "Wystąpił nieoczekiwany błąd.";
				throw;
			}
			finally
			{
				ShowLoadingIcon = false;
				RunLoadingIcon = false;
				BlockInteraction = false;

				_logger.LogInformation(
					"Zakończono proces edycji kategorii funduszu. FundCategoryId={FundCategoryId}",
					FundCategory.Id
				);
			}
		}



		private bool CheckShouldBe()
		{
			_logger.LogInformation(
				"Rozpoczynam sprawdzanie kwoty kategorii funduszy. ShouldBe={ShouldBe}",
				FundCategoryRequest.ShouldBe
			);

			if (FundCategoryRequest.ShouldBe <= 0)
			{
				_logger.LogWarning(
					"Kwota kategorii funduszy jest mniejsza lub równa zero. ShouldBe={ShouldBe}",
					FundCategoryRequest.ShouldBe
				);
				return false;
			}

			string amountStr = FundCategoryRequest.ShouldBe.ToString();
			var amountParts = amountStr.Split('.');

			if (amountParts[0].Length > 15)
			{
				_logger.LogWarning(
					"Kwota kategorii funduszy przekracza 15 cyfr przed przecinkiem. ShouldBe={ShouldBe}",
					FundCategoryRequest.ShouldBe
				);
				return false;
			}

			if (amountParts.Length == 2 && amountParts[1].Length > 2)
			{
				_logger.LogWarning(
					"Kwota kategorii funduszy przekracza 2 miejsca po przecinku. ShouldBe={ShouldBe}",
					FundCategoryRequest.ShouldBe
				);
				return false;
			}

			_logger.LogInformation(
				"Kwota kategorii funduszy jest poprawna. ShouldBe={ShouldBe}",
				FundCategoryRequest.ShouldBe
			);

			return true;
		}

		public async Task CancelEdit()
		{
			_logger.LogInformation(
				"Rozpoczynam anulowanie edycji funduszu (powrót do historii funduszy). FundId={FundId}, Amount={Amount}",
				FundCategory?.Id,
				FundCategory?.ShouldBe
			);

			try
			{
				await Shell.Current.GoToAsync("..");

				_logger.LogInformation(
					"Nawigacja po anulowaniu edycji funduszu zakończona sukcesem. FundId={FundId}",
					FundCategory?.Id
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"Nieoczekiwany błąd podczas anulowania edycji funduszu. FundId={FundId}",
					FundCategory?.Id
				);
				throw;
			}
		}



		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}

