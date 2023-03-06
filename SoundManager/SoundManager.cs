using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Audio players components.
    [SerializeField]
    private AudioSource m_BGSoundSource;
    [SerializeField]
    private AudioSource m_ThemeSound;
    [SerializeField]
    private AudioSource m_PlaceTilePiece;
    [SerializeField]
    private AudioSource m_RotateTilePiece;
    [SerializeField]
    private AudioSource m_PlaceBomb;
    [SerializeField]
    private AudioSource m_MovePiece;
    [SerializeField]
    private AudioSource m_CountDownSound;
    [SerializeField]
    private AudioSource m_LiquidFilled;
    [SerializeField]
    private AudioSource m_WindowTransition;
    [SerializeField]
    private AudioSource m_MouseHover;
    [SerializeField]
    private AudioSource m_CloseWindow;
    [SerializeField]
    private AudioClip[] m_PlaceTilesClips;
    [SerializeField]
    private AudioSource m_GameOver;
    [SerializeField]
    private AudioSource m_RotatePlayer1;
    [SerializeField]
    private AudioSource m_RotatePlayer2;
    [SerializeField]
    private AudioSource m_PauseMenu;
    [SerializeField]
    private AudioSource m_GoSound;


    public static SoundManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }
    public void PlayPausePanelSound()
    {
        m_PauseMenu.PlayOneShot(m_PauseMenu.clip);
    }
    public void PlayGOSound()
    {
        m_GoSound.PlayOneShot(m_GoSound.clip);
    }

    public void PlayBGSound()
    {
        if (!m_BGSoundSource.isPlaying)
            m_BGSoundSource.Play();
    }
    public void StopBGSound()
    {
        m_BGSoundSource.Stop();
    }

    public void PlayThemeSound()
    {
        if (!m_ThemeSound.isPlaying)
            m_ThemeSound.Play();
    }
    public void StopThemeSound()
    {
        m_ThemeSound.Stop();
    }
    public void PlayTilePlaceSound()
    {
        m_PlaceTilePiece.clip = m_PlaceTilesClips[Random.Range(Constants.INT_ZERO, m_PlaceTilesClips.Length)];
        m_PlaceTilePiece.PlayOneShot(m_PlaceTilePiece.clip);
    }

    public void PlayRotateTilePieceSound()
    {
        m_RotateTilePiece.PlayOneShot(m_RotateTilePiece.clip);
    }
    
    public void PlayRotateTilePieceSoundPlayer1()
    {
        m_RotatePlayer1.PlayOneShot(m_RotatePlayer1.clip);
    }
    public void PlayRotateTilePieceSoundPlayer2()
    {
        m_RotatePlayer2.PlayOneShot(m_RotatePlayer2.clip);
    }

    public void PlayPlaceBombSound()
    {
        m_PlaceBomb.PlayOneShot(m_PlaceBomb.clip);
    }

    public void PlayMovePieceSound()
    {
        m_MovePiece.PlayOneShot(m_MovePiece.clip);
    }

    public void PlayCountDownSound()
    {
        m_CountDownSound.PlayOneShot(m_CountDownSound.clip);
    }
    public void PlayLiquidFilledSound()
    {
        m_LiquidFilled.PlayOneShot(m_LiquidFilled.clip);
    }
    public void PlayWindowTransitionSound()
    {
        m_WindowTransition.PlayOneShot(m_WindowTransition.clip);
    }
    public void PlayMouseHoverSound()
    {
        m_MouseHover.PlayOneShot(m_MouseHover.clip);
    }
    public void PlayCloseWindowSound()
    {
        m_CloseWindow.PlayOneShot(m_CloseWindow.clip);
    }
    public void PlayGameOverSound()
    {
        m_GameOver.PlayOneShot(m_GameOver.clip);
    }

}
